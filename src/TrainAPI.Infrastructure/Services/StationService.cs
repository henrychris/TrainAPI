using TrainAPI.Application.Contracts;
using TrainAPI.Domain.Entities;
using TrainAPI.Infrastructure.Data;

namespace TrainAPI.Infrastructure.Services;

public class StationService(DataContext context) : IStationService
{
    public async Task CreateStation(Station station)
    {
        await context.Stations.AddAsync(station);
        await context.SaveChangesAsync();
    }

    public async Task<Station?> GetStation(string stationId)
    {
        return await context.Stations.FindAsync(stationId);
    }

    public async Task UpdateStation(Station station)
    {
        context.Stations.Update(station);
        await context.SaveChangesAsync();
    }

    public async Task DeleteStation(Station station)
    {
        context.Remove(station);
        await context.SaveChangesAsync();
    }

    public IQueryable<Station> GetQueryable()
    {
        return context.Stations.AsQueryable();
    }
}