using ErrorOr;

using MediatR;

using TrainAPI.Application.Contracts;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Trips.DeleteTrip;

public class DeleteTripRequest : IRequest<ErrorOr<Deleted>>
{
    public string TripId { get; set; } = string.Empty;
}

public class DeleteTripRequestHandler(ITripService tripService, ILogger<DeleteTripRequestHandler> logger)
    : IRequestHandler<DeleteTripRequest, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteTripRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Received request to delete trip. ID: {id}", request.TripId);

        var trip = await tripService.GetTrip(request.TripId);
        if (trip is null)
        {
            logger.LogError("Trip not found. ID: {id}", request.TripId);
            return SharedErrors<Trip>.NotFound;
        }

        await tripService.DeleteTrip(trip);
        logger.LogInformation("Successfully deleted Trip with ID: {id}", request.TripId);
        return Result.Deleted;
    }
}