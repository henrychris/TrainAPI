using TrainAPI.Application.Features.Trips.CreateTrip;
using TrainAPI.Application.Features.Trips.GetSingleTrip;
using TrainAPI.Application.Features.Trips.UpdateTrip;
using TrainAPI.Domain.Entities;

namespace TrainAPI.Application.Features.Trips;

public static class TripMapper
{
    public static Trip CreateTrip(CreateTripRequest request)
    {
        return new Trip
        {
            Name = request.Name,
            DateOfTrip = request.DateOfTrip,
            ArrivalTime = request.ArrivalTime,
            DepartureTime = request.DepartureTime,
            TrainId = request.TrainId,
            ToStationId = request.ToStationId,
            FromStationId = request.FromStationId,
            DistanceInKilometers = request.DistanceInKilometers,
        };
    }

    public static CreateTripResponse ToCreateTripResponse(Trip trip)
    {
        return new CreateTripResponse { TripId = trip.Id };
    }

    public static GetTripResponse ToGetTripResponse(Trip trip)
    {
        return new GetTripResponse
        {
            Name = trip.Name,
            DateOfTrip = trip.DateOfTrip,
            ArrivalTime = trip.ArrivalTime,
            DepartureTime = trip.DepartureTime,
            DistanceInKilometers = trip.DistanceInKilometers,
            TrainId = trip.TrainId,
            FromStationId = trip.FromStationId,
            ToStationId = trip.ToStationId
        };
    }

    public static UpdateTripRequest ToUpdateTripRequest(UpdateTripRequestDto requestDto, string tripId)
    {
        return new UpdateTripRequest
        {
            Name = requestDto.Name,
            ArrivalTime = requestDto.ArrivalTime,
            DateOfTrip = requestDto.DateOfTrip,
            DepartureTime = requestDto.DepartureTime,
            DistanceInKilometers = requestDto.DistanceInKilometers,
            TripId = tripId,
            FromStationId = requestDto.FromStationId,
            ToStationId = requestDto.ToStationId,
            TrainId = requestDto.TrainId
        };
    }
}