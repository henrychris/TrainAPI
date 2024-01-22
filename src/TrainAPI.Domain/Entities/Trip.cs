namespace TrainAPI.Domain.Entities
{
    public class Trip
    {
        // todo: there should be a feature to set recurring trips
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Name { get; set; }
        public required DateTime Date { get; set; } // todo: rename to date of trip
        public required DateTime ArrivalTime { get; set; }
        public required DateTime DepartureTime { get; set; }
        public required int DistanceInKilometers { get; set; }

        // navigation properties

        public required string TrainId { get; set; }
        public Train Train { get; set; } = null!;

        public required string ToStationId { get; set; }
        public Station ToStation { get; set; } = null!;

        public required string FromStationId { get; set; }
        public Station FromStation { get; set; } = null!;

    }
}