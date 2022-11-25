using System.Threading.Tasks;
using GrainInterface;
using Orleans;

namespace Grains.Counter
{
    public class Counter : Grain, ICounterGrain
    {
        private int _counter;

        public Task Increment(int increment)
        {
            _counter += increment;
            return Task.CompletedTask;
        }

        public Task<int> GetCount()
        {
            return Task.FromResult(_counter);
        }
    }
}
