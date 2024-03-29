﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains.BankAccount
{
    [Serializable]
    [GenerateSerializer, Immutable]
    public class BankAccountState
    {
        [Id(0)]
        public decimal Balance { get; set; }

        public BankAccountState Apply(Deposited evnt)
        {
            Balance += evnt.Amount;
            return this;
        }

        public BankAccountState Apply(Withdrawn evnt)
        {
            Balance -= evnt.Amount;
            return this;
        }
    }
}
