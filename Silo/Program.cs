using System;
using System.IO;
using System.Threading.Tasks;
using Grains;
using Grains.InventoryItem;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Hosting;
using Orleans.Runtime.Configuration;
using Orleans.Storage;
using SqlStreamStore;

namespace Silo
{
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var host = new HostBuilder()
                   .UseOrleans(builder =>
                   {
                       builder
                        .UseLocalhostClustering(serviceId: "SmartCacheApp", clusterId: "Test")
                        .AddMemoryGrainStorageAsDefault()
                        .AddLogStorageBasedLogConsistencyProvider("LogStorage")
                        .ConfigureLogging(logging => logging.AddConsole());
                   })
                   .ConfigureServices(services => 
                   {
                       var settings = new MsSqlStreamStoreV3Settings(config.GetConnectionString("ESConnection"));
                       settings.Schema = "ES";
                       settings.DisableDeletionTracking = true;
                       var store = new MsSqlStreamStoreV3(settings);
                       store.CreateSchemaIfNotExists();

                       services.AddSingleton<IStreamStore>(store);
                   })
                   .ConfigureLogging((context, logging) =>
                   {
                       logging.AddConsole();
                   })
                   .Build();
            await host.RunAsync();

            return 0;
        }
    }
}
