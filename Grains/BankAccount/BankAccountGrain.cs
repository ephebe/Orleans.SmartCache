﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GrainInterface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Orleans;
using Orleans.EventSourcing;
using Orleans.Providers;
using SqlStreamStore;
using SqlStreamStore.Streams;

namespace Grains.BankAccount;

[LogConsistencyProvider(ProviderName = "LogStorage")]
public class BankAccountGrain : JournaledGrain<BankAccountState>, IBankAccountGrain
{
    private string _stream;
    private IStreamStore _store;

    const int Defaultport = 1113;

    public BankAccountGrain(IStreamStore streamStore)
    {
        _store = streamStore;
    }

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _stream = $"{GetType().Name}-{this.GetPrimaryKey()}";

        await Transactions();

        await ConfirmEvents();

        await base.OnActivateAsync(cancellationToken);
    }

    public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        _store.Dispose();
        return base.OnDeactivateAsync(reason, cancellationToken);
    }

    public async Task Transactions()
    {
        var endOfStream = false;
        var startVersion = 0;

        while (endOfStream == false)
        {
            var stream = await _store.ReadStreamForwards(_stream, startVersion, 10);
            endOfStream = stream.IsEnd;
            startVersion = stream.NextStreamVersion;

            foreach (var msg in stream.Messages)
            {
                switch (msg.Type)
                {
                    case "Deposited":
                        var depositedJson = await msg.GetJsonData();
                        var deposited = JsonConvert.DeserializeObject<Deposited>(depositedJson);
                        base.RaiseEvent(deposited);
                        break;
                    case "Withdrawn":
                        var withdrawnJson = await msg.GetJsonData();
                        var withdrawn = JsonConvert.DeserializeObject<Withdrawn>(withdrawnJson);
                        base.RaiseEvent(withdrawn);
                        break;
                }
            }
        }
    }

    public async Task Deposit(decimal amount)
    {
        await RaiseEvent(new Deposited
        {
            Amount = amount
        });
    }

    public async Task Withdraw(decimal amount)
    {
        await RaiseEvent(new Withdrawn
        {
            Amount = amount
        });
    }

    public Task<decimal> Balance()
    {
        return Task.FromResult(State.Balance);
    }

    private async Task RaiseEvent(BankAccountEvent evnt)
    {
        base.RaiseEvent(evnt);
        await ConfirmEvents();

        try
        {
            await _store.AppendToStream(_stream, ExpectedVersion.Any, new NewStreamMessage(Guid.NewGuid(), evnt.GetType().Name, JsonConvert.SerializeObject(evnt)));
        }
        catch (Exception wx)
        {
            throw wx;
        }

    }
}
