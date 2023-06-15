using MassTransit;
using MassTransitClient.Consumers.Consumers;
using MassTransitClient.Domain.Providers.MessageBus;
using MassTransitClient.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Reflection;

namespace MassTransitClient.Infrastructure.Configuration
{
    public static partial class ServiceRegistrations
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddMessageBusServices(configuration);
            return serviceCollection;
        }

        public static IServiceCollection AddMessageBusServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            //Bind Mass transit settings
            var massTransitOptions = new MassTransitOptions();
            configuration.GetSection(MassTransitOptions.SectionName).Bind(massTransitOptions);

            // Bind RabbitMQ settings
            var rabbitMQOptions = new RabbitMQOptions();
            configuration.GetSection(RabbitMQOptions.SectionName).Bind(rabbitMQOptions);

            serviceCollection.AddMassTransit(x =>
            {
                // Register consumer based on assembly
                var entryAssembly = typeof(MessageConsumer).GetTypeInfo().Assembly;
                x.SetKebabCaseEndpointNameFormatter();
                x.AddConsumers(entryAssembly);

                x.UsingRabbitMq((context, cfg) =>
                {
                    // Configure RabbitMQ host
                    cfg.Host(rabbitMQOptions.Host, rabbitMQOptions.VirtualHost, h => {
                        h.Username(rabbitMQOptions.Username);
                        h.Password(rabbitMQOptions.Password);
                    });

                    // Configure endpoints
                    cfg.ConfigureEndpoints(context);

                    // Retry
                    cfg.UseMessageRetry(r => r.Incremental(massTransitOptions.IntervalLimit,
                                                           TimeSpan.FromSeconds(massTransitOptions.InitialInterval),
                                                           TimeSpan.FromSeconds(massTransitOptions.IntervalIncrement)));

                    // Circuit Breaker
                    cfg.UseCircuitBreaker(cb =>
                    {
                        cb.TrackingPeriod = TimeSpan.FromMinutes(massTransitOptions.TrackingPeriod);
                        cb.TripThreshold = massTransitOptions.TripThreshold;
                        cb.ResetInterval = TimeSpan.FromMinutes(massTransitOptions.ResetInterval);
                        cb.ActiveThreshold = massTransitOptions.ActiveThreshold;
                    });
                });
            });

            // Register Publisher
            serviceCollection.AddTransient<IPublishProvider, MassTransitPublisher>();

            return serviceCollection;
        }

        public static IHostBuilder AddSerilog(this IHostBuilder builder, string applicationName)
        {
            builder.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.WithProperty("ApplicationName", applicationName)
            .Enrich.FromLogContext()
            .WriteTo.Console());

            return builder;
        }
    }
}
