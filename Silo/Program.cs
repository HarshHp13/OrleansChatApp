

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using Orleans.Streams.Kafka.Config;
using Silo.Services;

class Program
{
    static async Task Main(string[] args)
    {
        var app=new HostBuilder().
            ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(serverOptions =>
                {
                    serverOptions.ListenLocalhost(5001, o =>
                    {
                        o.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
                        
                    });
                    serverOptions.Limits.Http2.KeepAlivePingDelay = TimeSpan.FromMinutes(1);
                    serverOptions.Limits.Http2.KeepAlivePingTimeout = TimeSpan.FromSeconds(30);
                }).ConfigureServices(services =>
                {
                    services.AddGrpc(options =>
                    {
                        options.EnableDetailedErrors = true;
                    });
                    services.AddGrpcReflection();
                    services.AddSingleton<IGrainFactory>(provider => provider.GetRequiredService<IClusterClient>());
                }).Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapGrpcService<MessageService>();
                        endpoints.MapGrpcReflectionService();
                    });
                });
            })
            .UseOrleans(siloBuilder =>
            {
                //Presistent State Storage
                siloBuilder.UseAzureStorageClustering(options =>
                {
                    options.TableServiceClient = new Azure.Data.Tables.TableServiceClient("UseDevelopmentStorage=true;");
                }).AddAzureTableGrainStorage("tableStorage",options =>
                {
                    options.TableServiceClient = new Azure.Data.Tables.TableServiceClient("UseDevelopmentStorage=true;");
                });

                //Kafka
                siloBuilder.AddKafkaStreamProvider("KafkaStreamProvider", options =>
                {
                    options.BrokerList = new[] { "localhost:9092" };
                    options.AddTopic("Message", new TopicCreationConfig
                    {
                        AutoCreate = true,
                        Partitions = 10,
                        ReplicationFactor = 1,
                        RetentionPeriodInMs = 1080000, // 3 hrs
                    });
                    options.SecurityProtocol = SecurityProtocol.Plaintext;
                }).AddAzureTableGrainStorage("PubSubStore", options =>
                {
                    options.TableServiceClient = new Azure.Data.Tables.TableServiceClient("UseDevelopmentStorage=true;");
                });

                siloBuilder.Configure<ClusterOptions>(options => {
                    options.ClusterId = "chat-app";
                    options.ServiceId = "chat-app";
                });

            })
            .Build();
        app.Run();
        
    }
}
