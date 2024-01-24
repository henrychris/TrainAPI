using System.ComponentModel.DataAnnotations.Schema;

namespace TrainAPI.Domain.Entities
{
    public class Coach
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string Name { get; set; }
        public required string Class { get; set; }
        public required int SeatCount { get; set; }
        public int AvailableSeats => Seats.Count(s => !s.IsBooked);
        [Column(TypeName = "jsonb")] public List<Seat> Seats { get; set; } = [];

        [Column(TypeName = "jsonb")]
        public required List<TravellerPairs> TravellerCategories { get; set; } // todo: validate json structure

        // navigation properties
        public required string TrainId { get; set; }
        public Train Train { get; set; } = null!;

        public void InitializeSeats(int numberOfSeats)
        {
            for (int i = 1; i <= numberOfSeats; i++)
            {
                Seats.Add(new Seat { SeatNumber = i });
            }
        }
    }
}