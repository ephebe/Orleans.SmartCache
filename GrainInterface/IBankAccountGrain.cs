namespace GrainInterface;

public interface IBankAccountGrain : IGrainWithGuidKey
{
    Task Deposit(decimal amount);
    Task Withdraw(decimal amount);
    Task<decimal> Balance();
}
