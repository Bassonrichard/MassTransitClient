namespace MassTransitClient.Infrastructure.Options
{
    public sealed class MassTransitOptions
    {
        public const string SectionName = "MassTransit";
        public int IntervalLimit { get; set; }
        public int InitialInterval { get; set; }
        public int IntervalIncrement { get; set; }
        public int TrackingPeriod { get; set; }
        public int TripThreshold { get; set; }
        public int ResetInterval { get; set; }
        public int ActiveThreshold { get; set; }
    }
}
