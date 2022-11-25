using System.Threading.Tasks;
using GrainInterface;
using Orleans;

namespace Grains.StateHolder
{

    public abstract class StateHolderGrain<T> : Grain<StateHolderStatus<T>>,
        IStateHolderGrain<T>
    {
        public Task<T> GetItem()
        {
            return Task.FromResult(State.Value);
        }

        public async Task<T> SetItem(T item)
        {
            State.Value = item;
            await WriteStateAsync();

            return State.Value;
        }
    }
}
