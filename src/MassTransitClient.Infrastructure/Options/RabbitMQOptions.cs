namespace MassTransitClient.Infrastructure.Options
{
    public class RabbitMQOptions
    {
        public const string SectionName = "RabbitMQ";
        public string Host { get; set; }
        public string VirtualHost { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
