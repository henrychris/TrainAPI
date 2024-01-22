using MediatR;

using ErrorOr;

using TrainAPI.Application.Contracts;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Trips.GetSingleTrip;

public class GetSingleTripRequest : IRequest<ErrorOr<GetTripResponse>>
{
    public string TripId { get; set; } = string.Empty;
}

public class GetSingleTripRequestHandler(
    ITripService tripService,
    ILogger<GetSingleTripRequestHandler> logger)
    : IRequestHandler<GetSingleTripRequest, ErrorOr<GetTripResponse>>
{
    public async Task<ErrorOr<GetTripResponse>> Handle(GetSingleTripRequest request,
                                                       CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching trip with Id: {id}.", request.TripId);

        var trip = await tripService.GetTrip(request.TripId);
        if (trip is null)
        {
            logger.LogError("Trip not found. Id: {id}.", request.TripId);
            return SharedErrors<Trip>.NotFound;
        }

        logger.LogInformation("Successfully fetched Trip with ID: {id}", request.TripId);
        return TripMapper.ToGetTripResponse(trip);
    }
}