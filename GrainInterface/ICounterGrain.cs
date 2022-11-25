using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GrainInterface
{
    public interface ICounterGrain : IGrainWithStringKey
    {
        Task Increment(int increment);
        Task<int> GetCount();
    }
}
