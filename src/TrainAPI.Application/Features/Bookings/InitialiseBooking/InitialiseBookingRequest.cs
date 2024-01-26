using ErrorOr;

using FluentValidation;

using MediatR;

using TrainAPI.Application.Contracts;
using TrainAPI.Application.Extensions;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Bookings.InitialiseBooking;

public class InitialiseBookingRequest : IRequest<ErrorOr<InitialiseBookingResponse>>
{
    public string TripId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public ContactDetails ContactInfo { get; set; } = null!;
    public List<Passenger> Passengers { get; set; } = null!;
}

public class
    InitialiseBookingRequestHandler(
        IBookingService bookingService,
        ITripService tripService,
        ILogger<InitialiseBookingRequestHandler> logger,
        IValidator<InitialiseBookingRequest> validator)
    : IRequestHandler<InitialiseBookingRequest, ErrorOr<InitialiseBookingResponse>>
{
    public async Task<ErrorOr<InitialiseBookingResponse>> Handle(InitialiseBookingRequest request,
                                                                 CancellationToken cancellationToken)
    {
        logger.LogInformation("Received a booking request. {request}", request);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogError("Validation failed for {request}. Errors: {errors}", nameof(InitialiseBookingRequest),
                validationResult.ToErrorList());
            return validationResult.ToErrorList();
        }

        var trip = await tripService.GetTrip(request.TripId);
        if (trip is null)
        {
            logger.LogError("The trip does not exist. Id: {id}", request.TripId);
            return SharedErrors<Trip>.NotFound;
        }

        var areSeatsValid = await ValidatePassengerSeats(request.Passengers);
        if (areSeatsValid.IsError)
        {
            logger.LogError("Some seats are not available. Errors: {errors}", areSeatsValid.Errors);
            return areSeatsValid.Errors;
        }


        Booking booking = BookingMapper.CreateBookingResponse(request);
        await bookingService.CreateBooking(booking);

        await bookingService.TemporarilyReserveSeats(request.Passengers, booking.Id);

        logger.LogInformation("Successfully created a booking. {booking}", booking);
        return BookingMapper.CreateBookingResponse(booking);
    }

    private async Task<ErrorOr<bool>> ValidatePassengerSeats(List<Passenger> passengers)
    {
        var errors = new List<Error>();

        foreach (var passenger in passengers)
        {
            var seatNo = passenger.SeatNumber;
            var coachId = passenger.CoachId;

            bool isSeatAvailable = await bookingService.IsSeatAvailable(seatNo, coachId);
            if (!isSeatAvailable)
            {
                errors.Add(Error.Validation(
                    code: "Booking.SeatNotAvailable",
                    description: $"Seat {seatNo} in coach {coachId} is not available."));
            }
        }

        return errors.Count == 0 ? true : errors;
    }
}