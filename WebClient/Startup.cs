using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Hosting;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var clusterClient = CreateOrleansClient();
            services.AddSingleton(provider => clusterClient);

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }

        private IClusterClient CreateOrleansClient() 
        {
            return Policy<IClusterClient>
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
                            parts.AddApplicationPart(typeof(BankAccountGrain).Assembly).WithReferences();
                        })
                        .ConfigureLogging(logging => logging.AddConsole());

                    var client = builder.Build();
                    client.Connect().Wait();
                    return client;
                });
        }
    }
}
