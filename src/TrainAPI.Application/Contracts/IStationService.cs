using TrainAPI.Domain.Entities;

namespace TrainAPI.Application.Contracts;

public interface IStationService
{
    Task CreateStation(Station station);
    Task<Station?> GetStation(string stationId);
    Task UpdateStation(Station station);
    Task DeleteStation(Station station);
    IQueryable<Station> GetQueryable();
    Task<(bool fromStationExists, bool toStationExists)> DoStationsExist(string fromStationId, string toStationId);
}