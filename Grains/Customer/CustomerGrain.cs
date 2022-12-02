using System;
using System.Threading.Tasks;
using GrainInterface;
using Orleans.Runtime;

namespace Grains.Customer;

public class CustomerGrain : Grain, ICustomerGrain 
{
    private readonly IPersistentState<CustomerState> _customer;

    public CustomerGrain(
       [PersistentState(
            stateName: "Customer")]
        IPersistentState<CustomerState> customer) => _customer = customer;

    public Task<CustomerState> GetItem()
    {
        return Task.FromResult(_customer.State);
    }

    public async Task<CustomerState> SetItem(CustomerState customer)
    {
        _customer.State = customer;
        await _customer.WriteStateAsync();

        return _customer.State;
    }
}
