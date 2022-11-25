using GrainInterface;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System;
using System.Threading.Tasks;

namespace WebClient.Controllers
{
    [Route("inventory")]
    [ApiController]
    public class InventoryItemController : Controller
    {
        IClusterClient _clusterClient = null;
        public InventoryItemController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        [Route("{Id:Guid}")]
        [HttpGet]
        public async Task<int> GetQuantity(Guid Id) 
        {
            var grain = _clusterClient.GetGrain<IInventoryItemGrain>(Id);
            var currentQty = await grain.Quantity();
            return currentQty;
        }

        [HttpPost]
        public IActionResult CreateNew() 
        {
            var inventoryItemId = Guid.NewGuid();
            var grain = _clusterClient.GetGrain<IInventoryItemGrain>(inventoryItemId);

            return Created("/inventory/{inventoryItemId}", new { inventoryItemId = inventoryItemId });
        }

        [Route("{Id:Guid}/increment")]
        [HttpPost]
        public async Task<int> Increment([FromQuery]Guid Id,[FromBody] InventoryRequest request ) 
        {
            var grain = _clusterClient.GetGrain<IInventoryItemGrain>(Id);

            if (request.Quantity >= 0)
            {
                await grain.Increment(request.Quantity);
            }

            var currentQty = await grain.Quantity();
            return currentQty;
        }
    }

    public class InventoryRequest
    {
        public int Quantity { get; set; }
    }
}
