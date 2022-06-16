using Grains;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DemoOrleansClient.ClusterClient = Policy<IClusterClient>
                .Handle<Exception>()
                .WaitAndRetry(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(3)
                })
                .Execute(() =>
                {
                    var builder = new ClientBuilder()
                        .UseLocalhostClustering(serviceId: "SmartCacheApp", clusterId: "Test")
                        .ConfigureApplicationParts(parts => 
                        {
                            parts.AddApplicationPart(typeof(IInventoryItemGrain).Assembly).WithReferences();
                        })    
                        .ConfigureLogging(logging => logging.AddConsole());

                    var client = builder.Build();
                    client.Connect().Wait();
                    return client;
                });

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
