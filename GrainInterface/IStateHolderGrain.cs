
namespace GrainInterface;

public interface IStateHolderGrain<T> : IGrainWithGuidKey
{
    Task<T> GetItem();
    Task<T> SetItem(T obj);
}
