using System;
using System.Collections.Generic;
using System.Text;

namespace GrainInterface;

public interface ICustomerGrain : IGrainWithGuidKey
{
    Task<CustomerState> GetItem();
    Task<CustomerState> SetItem(CustomerState customer);
}
