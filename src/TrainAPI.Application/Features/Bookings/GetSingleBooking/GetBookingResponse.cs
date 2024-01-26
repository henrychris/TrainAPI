namespace TrainAPI.Application.Features.Bookings.GetSingleBooking
{
    public class GetBookingResponse
    {
        public required string Id { get; set; }
        public required string Status { get; set; }
        public required bool IsConfirmed { get; set; }
    }
}