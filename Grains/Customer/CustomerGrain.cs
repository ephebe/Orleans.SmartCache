using System;
using System.Threading.Tasks;
using GrainInterface;
using Grains.StateHolder;
using Orleans.Providers;

namespace Grains.Customer;


[StorageProvider(ProviderName = "MemoryStore")]
public class CustomerGrain : StateHolderGrain<CustomerState>, ICustomerGrain 
{

}
