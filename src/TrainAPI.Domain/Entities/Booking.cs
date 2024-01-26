using TrainAPI.Domain.Enums;

namespace TrainAPI.Domain.Entities
{
    public class Booking
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Status { get; set; } = BookingStatus.UnConfirmed.ToString();
        public bool IsConfirmed { get; set; }

    }
}