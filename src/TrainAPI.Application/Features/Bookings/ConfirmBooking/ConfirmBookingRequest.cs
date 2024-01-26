using ErrorOr;

using Hangfire;

using MediatR;

using TrainAPI.Application.Contracts;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.Enums;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Bookings.ConfirmBooking
{
    public class ConfirmBookingRequest : IRequest<ErrorOr<ConfirmBookingResponse>>
    {
        public string BookingId { get; set; } = string.Empty;
    }

    public class ConfirmBookingRequestHandler(IBookingService bookingService) : IRequestHandler<ConfirmBookingRequest, ErrorOr<ConfirmBookingResponse>>
    {
        public async Task<ErrorOr<ConfirmBookingResponse>> Handle(ConfirmBookingRequest request, CancellationToken cancellationToken)
        {
            // todo: normally this would be a payment being validated
            var booking = await bookingService.GetBookingAsync(request.BookingId);
            if (booking is null)
            {
                return SharedErrors<Booking>.NotFound;
            }

            if (booking.IsConfirmed)
            {
                return Errors.Booking.BookingAlreadyConfirmed;
            }

            if (booking.Status == BookingStatus.Expired.ToString())
            {
                return Errors.Booking.BookingAlreadyExpired;
            }
            
            booking.IsConfirmed = true;
            await bookingService.UpdateBookingAsync(booking);
            
            if (booking.JobId is not null)
            {
                BackgroundJob.Delete(booking.JobId);
            }
            
            return BookingMapper.CreateConfirmBookingResponse(booking);
        }
    }
}