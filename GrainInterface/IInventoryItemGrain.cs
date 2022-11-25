using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GrainInterface
{
    public interface IInventoryItemGrain : IGrainWithGuidKey
    {
        Task Increment(int qty);
        Task Decrement(int qty);
        Task<int> Quantity();
    }
}
