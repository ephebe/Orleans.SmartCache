using GrainInterface;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Threading.Tasks;

namespace WebClient.Controllers
{
    [Route("counter")]
    [ApiController]
    public class CounterController : Controller
    {
        IClusterClient _clusterClient = null;
        public CounterController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        [HttpGet]
        public async Task<string> GetCounter()
        {
            var counter = _clusterClient.GetGrain<ICounterGrain>("Demo");
            var currentCount = await counter.GetCount();
            return currentCount.ToString();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCounter()
        {
            var counter = _clusterClient.GetGrain<ICounterGrain>("Demo");
            await counter.Increment(1);
            return NoContent();
        }
    }
}
