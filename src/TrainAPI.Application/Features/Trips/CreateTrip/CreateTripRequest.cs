using ErrorOr;

using FluentValidation;

using MediatR;

using TrainAPI.Application.Contracts;
using TrainAPI.Application.Extensions;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Trips.CreateTrip;

public class CreateTripRequest : IRequest<ErrorOr<CreateTripResponse>>
{
    public string Name { get; set; } = string.Empty;
    public DateTime DateOfTrip { get; set; }
    public DateTime ArrivalTime { get; set; }
    public DateTime DepartureTime { get; set; }

    public int DistanceInKilometers { get; set; }

    public string TrainId { get; set; } = string.Empty;
    public string ToStationId { get; set; } = string.Empty;
    public string FromStationId { get; set; } = string.Empty;
}

public class CreateTripRequestHandler(
    ITripService tripService,
    IStationService stationService,
    ITrainService trainService,
    ILogger<CreateTripRequestHandler> logger,
    IValidator<CreateTripRequest> validator) : IRequestHandler<CreateTripRequest, ErrorOr<CreateTripResponse>>
{
    public async Task<ErrorOr<CreateTripResponse>> Handle(CreateTripRequest request,
                                                          CancellationToken cancellationToken)
    {
        logger.LogInformation("Trying to create a new trip. Request: {request}", request);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.ToErrorList();
            logger.LogError("Validation failed for {request}. Errors: {errors}", nameof(CreateTripRequest),
                errors);
            return errors;
        }

        if (await trainService.GetTrain(request.TrainId) is null)
        {
            logger.LogError("The selected train does not exist. Id: {id}", request.TrainId);
            return SharedErrors<Train>.NotFound;
        }

        if (await tripService.AreTripsClashing(request))
        {
            logger.LogError("The trip seems to be clashing with some others.");
            return Errors.Trip.ClashingTrips;
        }

        (bool fromStationExists, bool toStationExists) =
            await stationService.DoStationsExist(request.FromStationId, request.ToStationId);
        if (!fromStationExists)
        {
            logger.LogError("The 'from' station does not exist.");
            return Errors.Trip.FromStationDoesNotExist;
        }

        if (!toStationExists)
        {
            logger.LogError("The 'to' station does not exist.");
            return Errors.Trip.ToStationDoesNotExist;
        }

        Trip trip = TripMapper.CreateTrip(request);
        await tripService.CreateTrip(trip);

        logger.LogInformation("Trip Created. Id: {id}", trip.Id);
        return TripMapper.ToCreateTripResponse(trip);
    }
}