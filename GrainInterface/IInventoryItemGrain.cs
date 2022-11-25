namespace GrainInterface;

public interface IInventoryItemGrain : IGrainWithGuidKey
{
    Task Increment(int qty);
    Task Decrement(int qty);
    Task<int> Quantity();
}
