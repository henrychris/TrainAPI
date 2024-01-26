namespace TrainAPI.Domain.Entities.Other
{
    public class UnreserveSeatRequest
    {
        public required string BookingId { get; set; }
        public required Seat Seat { get; set; }
        public required string CoachId { get; set; }

    }
}