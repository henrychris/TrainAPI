using ErrorOr;

using TrainAPI.Application.Features.Bookings.ConfirmBooking;

using TrainAPI.Application.Features.Bookings.InitialiseBooking;
using TrainAPI.Domain.Entities;

namespace TrainAPI.Application.Features.Bookings
{
    public static class BookingMapper
    {
        internal static Booking CreateBookingResponse(InitialiseBookingRequest request)
        {
            return new Booking();
        }

        internal static InitialiseBookingResponse CreateBookingResponse(Booking booking)
        {
            return new InitialiseBookingResponse
            {
                BookingId = booking.Id
            };
        }

        internal static ConfirmBookingResponse CreateConfirmBookingResponse(Booking booking)
        {
            return new ConfirmBookingResponse
            {
                BookingId = booking.Id
            };
        }
    }
}