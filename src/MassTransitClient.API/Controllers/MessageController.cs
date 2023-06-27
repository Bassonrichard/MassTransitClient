using MassTransitClient.Domain.Models;
using MassTransitClient.Domain.Providers.MessageBus;
using Microsoft.AspNetCore.Mvc;

namespace MassTransitClient.API.Controllers
{
    [ApiController]
    [Route("api/Message/")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly IPublishProvider _publishProvider;

        public MessageController(ILogger<MessageController> logger, IPublishProvider publishProvider)
        {
            _logger = logger;
            _publishProvider = publishProvider;
        }

        [HttpPost]
        [Route("Publish")]
        public async Task<IActionResult> Publish([FromBody] Message message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Message: {@message}", message);

            await _publishProvider.PublishAsync(message, null, cancellationToken);
            return Accepted();
        }

        [HttpPost]
        [Route("Send")]
        public async Task<IActionResult> Send([FromBody] Message message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Message: {@message}", message);

            await _publishProvider.SendAsync(message, "message", cancellationToken);
            return Accepted();
        }
    }
}