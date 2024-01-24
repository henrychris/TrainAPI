using ErrorOr;

using FluentValidation;

using MediatR;

using TrainAPI.Application.Contracts;
using TrainAPI.Application.Extensions;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Trips.UpdateTrip;

public class UpdateTripRequestDto
{
    public DateTime? DateOfTrip { get; set; }
    public DateTime? ArrivalTime { get; set; }
    public DateTime? DepartureTime { get; set; }
    public string? Name { get; set; }
    public string? TrainId { get; set; }
    public string? ToStationId { get; set; }
    public string? FromStationId { get; set; }
    public int? DistanceInKilometers { get; set; }
}

public class UpdateTripRequest : IRequest<ErrorOr<Updated>>
{
    public required string TripId { get; init; }
    public DateTime? DateOfTrip { get; init; }
    public DateTime? ArrivalTime { get; init; }
    public DateTime? DepartureTime { get; init; }
    public string? Name { get; init; }
    public string? TrainId { get; init; }
    public string? ToStationId { get; init; }
    public string? FromStationId { get; init; }
    public int? DistanceInKilometers { get; init; }
}

public class UpdateTripRequestHandler(
    ITripService tripService,
    IValidator<UpdateTripRequest> validator,
    ILogger<UpdateTripRequestHandler> logger) : IRequestHandler<UpdateTripRequest, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateTripRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Trying to update a trip. Request: {request}", request);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.ToErrorList();
            logger.LogError("Validation failed for {request}. Errors: {errors}", nameof(UpdateTripRequest),
                errors);
            return errors;
        }

        var trip = await tripService.GetTrip(request.TripId);
        if (trip is null)
        {
            logger.LogError("Trip not found. ID: {id}", request.TripId);
            return SharedErrors<Trip>.NotFound;
        }

        trip.Name = request.Name ?? trip.Name;
        trip.DateOfTrip = request.DateOfTrip ?? trip.DateOfTrip;
        trip.ArrivalTime = request.ArrivalTime ?? trip.ArrivalTime;
        trip.DepartureTime = request.DepartureTime ?? trip.DepartureTime;
        trip.TrainId = request.TrainId ?? trip.TrainId;
        trip.ToStationId = request.ToStationId ?? trip.ToStationId;
        trip.FromStationId = request.FromStationId ?? trip.FromStationId;
        trip.DistanceInKilometers = request.DistanceInKilometers ?? trip.DistanceInKilometers;

        await tripService.UpdateTrip(trip);
        logger.LogInformation("Trip {id} has been updated.", trip.Id);
        return Result.Updated;
    }
}