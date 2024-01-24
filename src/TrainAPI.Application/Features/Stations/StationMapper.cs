using TrainAPI.Application.Features.Stations.CreateStation;
using TrainAPI.Application.Features.Stations.GetStation;
using TrainAPI.Application.Features.Stations.UpdateStation;
using TrainAPI.Domain.Entities;

namespace TrainAPI.Application.Features.Stations;

public static class StationMapper
{
    public static CreateStationResponse ToCreateStationResponse(Station createdStation)
    {
        return new CreateStationResponse { StationId = createdStation.Id };
    }

    public static Station CreateStation(CreateStationRequest request)
    {
        return new Station { Code = request.Code, Name = request.Name };
    }

    public static GetStationResponse ToGetStationResponse(Station station)
    {
        return new GetStationResponse { Id = station.Id, Code = station.Code, Name = station.Name };
    }

    public static UpdateStationRequest ToUpdateStationRequest(UpdateStationRequestDto requestDto, string stationId)
    {
        return new UpdateStationRequest
        {
            StationId = stationId,
            Code = requestDto.Code,
            Name = requestDto.Name
        };
    }
}