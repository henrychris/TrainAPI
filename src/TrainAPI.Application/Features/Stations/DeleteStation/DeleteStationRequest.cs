using ErrorOr;

using MediatR;

using TrainAPI.Application.Contracts;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Stations.DeleteStation;

public class DeleteStationRequest : IRequest<ErrorOr<Deleted>>
{
    public string StationId { get; set; } = string.Empty;
}

public class DeleteStationRequestHandler(IStationService stationService, ILogger<DeleteStationRequestHandler> logger)
    : IRequestHandler<DeleteStationRequest, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteStationRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Received request to delete station. ID: {id}", request.StationId);

        var station = await stationService.GetStation(request.StationId);
        if (station is null)
        {
            logger.LogError("Station not found. ID: {id}", request.StationId);
            return SharedErrors<Station>.NotFound;
        }

        await stationService.DeleteStation(station);
        logger.LogInformation("Successfully deleted Station with ID: {id}", request.StationId);
        return Result.Deleted;
    }
}