namespace MassTransitClient.Domain.Providers.MessageBus
{
    public interface IPublishProvider
    {
        Task SendAsync<T>(T message, string queueName = default, CancellationToken cancellationToken = default) where T : class;
        Task PublishAsync<T>(T message, string queueName = default, CancellationToken cancellationToken = default) where T : class;
        Task PublishAsync<T>(T message, Dictionary<string, string> headers = default, string queueName = default, CancellationToken cancellationToken = default) where T : class;
    }
}
