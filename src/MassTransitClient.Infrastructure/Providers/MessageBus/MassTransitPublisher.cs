using MassTransit;
using MassTransitClient.Domain.Providers.MessageBus;

namespace MassTransitClient.Infrastructure.Configuration
{
    public class MassTransitPublisher : IPublishProvider
    {
        private readonly IBus _bus;

        public MassTransitPublisher(IBus bus) => _bus = bus;

        public async Task SendAsync<T>(T message, string queueName = null, CancellationToken cancellationToken = default) where T : class
        {
            var sendEndpoint = await GetSendEndpoint(queueName);
            await sendEndpoint.Send(message, cancellationToken);
        }

        public async Task PublishAsync<T>(T message, string queueName = default, CancellationToken cancellationToken = default) where T : class
        {
            await _bus.Publish(message, cancellationToken);
        }

        public async Task PublishAsync<T>(T message, Dictionary<string, string> headers = default, string queueName = default, CancellationToken cancellationToken = default) where T : class
        {
            await _bus.Publish(message, context =>
            {
                foreach (var header in headers)
                {
                    context.Headers.Set(header.Key, header.Value);
                }
            },
            cancellationToken);
        }

        private async Task<ISendEndpoint> GetSendEndpoint(string queueName)
        {
            var sendToUri = new Uri($"queue:{queueName}");
            return await _bus.GetSendEndpoint(sendToUri);
        }

    }
}