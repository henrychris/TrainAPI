using TrainAPI.Application.Features.Trips.CreateTrip;
using TrainAPI.Domain.Entities;

namespace TrainAPI.Application.Contracts;

public interface ITripService
{
    Task CreateTrip(Trip trip);

    Task<bool> AreTripsClashing(CreateTripRequest request);
    Task<Trip?> GetTrip(string requestTripId);
    Task DeleteTrip(Trip trip);
    IQueryable<Trip> GetQueryable();
    Task UpdateTrip(Trip trip);
}