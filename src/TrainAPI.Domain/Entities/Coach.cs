namespace TrainAPI.Domain.Entities
{
public class Coach
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string Name { get; set; }
        public required string Class { get; set; }
        public required int SeatCount { get; set; }
        public required int AvailableSeats { get; set; }

        public required List<TravellerPairs> TravellerCategories { get; set; } // todo: validate json structure
        // navigation properties
        public required string TrainId { get; set; }
        public Train Train { get; set; } = null!;
    }
}