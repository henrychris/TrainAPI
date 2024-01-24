using ErrorOr;

using MediatR;

using TrainAPI.Application.Contracts;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Stations.GetStation;

public class GetSingleStationRequest : IRequest<ErrorOr<GetStationResponse>>
{
    public string StationId { get; set; } = string.Empty;
}

public class GetSingleStationRequestHandler(
    IStationService stationService,
    ILogger<GetSingleStationRequestHandler> logger)
    : IRequestHandler<GetSingleStationRequest, ErrorOr<GetStationResponse>>
{
    public async Task<ErrorOr<GetStationResponse>> Handle(GetSingleStationRequest request,
                                                          CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching station with Id: {id}.", request.StationId);

        var station = await stationService.GetStation(request.StationId);
        if (station is null)
        {
            logger.LogError("Station not found. Id: {id}.", request.StationId);
            return SharedErrors<Station>.NotFound;
        }

        logger.LogInformation("Successfully fetched Station with ID: {id}", request.StationId);
        return StationMapper.ToGetStationResponse(station);
    }
}