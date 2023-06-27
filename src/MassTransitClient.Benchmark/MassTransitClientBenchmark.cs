using BenchmarkDotNet.Attributes;
using MassTransitClient.Domain.Models;
using MassTransitClient.Domain.Providers.MessageBus;
using MassTransitClient.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace MassTransitClient.Benchmark
{
    public class MassTransitClientBenchmark
    {
        private readonly IPublishProvider _publishProvider;
        private const int RequestsPerSecond = 1000;
        
        public MassTransitClientBenchmark()
        {
            var serviceProvider = CreateTestServiceProvider();
            _publishProvider = serviceProvider.GetRequiredService<IPublishProvider>();
        }


        [Benchmark(OperationsPerInvoke = RequestsPerSecond)]
        public async Task PublishAsync()
        {
            await _publishProvider.PublishAsync(new Message { Text = "" }, null, CancellationToken.None);
        }  

        [Benchmark(OperationsPerInvoke = RequestsPerSecond)]
        public async Task SendAsync()
        {
            await _publishProvider.SendAsync(new Message { Text = "" }, "message");
        }

        private static ServiceProvider CreateTestServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddInfrastructureServices(CreateConfiguration());
            return services.BuildServiceProvider();
        }

        private static IConfiguration CreateConfiguration()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.Tests.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}
