using System;
using System.Threading.Tasks;
using GrainInterface;
using Grains.StateHolder;
using Orleans.Providers;

namespace Grains.Customer
{

    [StorageProvider(ProviderName = "MemoryStore")]
    public class CustomerGrain : StateHolderGrain<CustomerState>, ICustomerGrain 
    {
    
    }

    //public static class CustomerGrain
    //{

    //    public static async Task CreateCustomer(Guid id, string name)
    //    {
    //        var state = new CustomerState
    //        {
    //            Id = id,
    //            Name = name
    //        };
    //        var grain = DemoOrleansClient.ClusterClient.GetGrain<ICustomerGrain>(id);
    //        await grain.SetItem(state);
    //    }

    //    public static async Task<CustomerState> GetCustomer(Guid id)
    //    {
    //        var grain = DemoOrleansClient.ClusterClient.GetGrain<ICustomerGrain>(id);
    //        return await grain.GetItem();
    //    }
    //}
}
