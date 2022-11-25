using GrainInterface;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System;
using System.Threading.Tasks;

namespace WebClient.Controllers
{
    [Route("customers")]
    [ApiController]
    public class CustomerController : Controller
    {
        IClusterClient _clusterClient = null;
        public CustomerController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        [Route("{customerId:Guid}")]
        [HttpGet]
        public async Task<CustomerState> GetCustomer([FromQuery] Guid customerId)
        {
            var grain =  _clusterClient.GetGrain<ICustomerGrain>(customerId);
            var customer = await grain.GetItem();

            return customer;
        }

        [HttpPost]
        public async Task<CustomerState> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            var customerId = Guid.NewGuid();
            var grain = _clusterClient.GetGrain<ICustomerGrain>(customerId);
            var customer = await grain.SetItem(new CustomerState
            {
                Id = customerId,
                Name = request.Name
            });

            return customer;
        }
    }

    public class CreateCustomerRequest 
    {
        public string Name { get; set; }
    }
}
