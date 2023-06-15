using MassTransit;
using MassTransitClient.Domain.Models;
using Microsoft.Extensions.Logging;

namespace MassTransitClient.Consumers.Consumers
{
    public class MessageConsumer : IConsumer<Message>
    {
        private readonly ILogger<MessageConsumer> _logger;

        public MessageConsumer(ILogger<MessageConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<Message> context)
        {
            try
            {
                _logger.LogInformation("Consumed message: {@message}", context.Message);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to consume message: {@message} due to: {@errorMessage}", context.Message, ex.Message);
                throw;
            }
        }
    }
}