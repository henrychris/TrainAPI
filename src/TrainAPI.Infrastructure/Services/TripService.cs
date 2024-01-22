using Microsoft.EntityFrameworkCore;

using TrainAPI.Application.Contracts;
using TrainAPI.Application.Features.Trips.CreateTrip;
using TrainAPI.Domain.Entities;
using TrainAPI.Infrastructure.Data;

namespace TrainAPI.Infrastructure.Services;

public class TripService(DataContext context) : ITripService
{
    public async Task CreateTrip(Trip trip)
    {
        await context.Trips.AddAsync(trip);
        await context.SaveChangesAsync();
    }

    public async Task<bool> AreTripsClashing(CreateTripRequest request)
    {
        return await context.Trips.AnyAsync(x =>
            x.FromStationId == request.FromStationId &&
            x.ToStationId == request.ToStationId &&
            x.ArrivalTime == request.ArrivalTime &&
            x.DepartureTime == request.DepartureTime);
    }

    public async Task<Trip?> GetTrip(string tripId)
    {
        return await context.Trips.FindAsync(tripId);
    }

    public async Task DeleteTrip(Trip trip)
    {
        context.Remove(trip);
        await context.SaveChangesAsync();
    }

    public IQueryable<Trip> GetQueryable()
    {
        return context.Trips.AsQueryable();
    }

    public async Task UpdateTrip(Trip trip)
    {
        context.Trips.Update(trip);
        await context.SaveChangesAsync();
    }
}