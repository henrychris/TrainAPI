using ErrorOr;

using MediatR;

namespace TrainAPI.Application.Features.Bookings.GetSingleBooking
{
    public class GetSingleBookingRequest : IRequest<ErrorOr<GetBookingResponse>>
    {
        public string BookingId { get; set; } = string.Empty;
    }
}