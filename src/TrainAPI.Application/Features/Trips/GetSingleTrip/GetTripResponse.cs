namespace TrainAPI.Application.Features.Trips.GetSingleTrip;

public class GetTripResponse
{
    public required string Name { get; init; }
    public required DateTime DateOfTrip { get; init; }
    public required DateTime ArrivalTime { get; init; }
    public required DateTime DepartureTime { get; init; }
    public required int DistanceInKilometers { get; init; }
    public required string TrainId { get; init; }
    public required string ToStationId { get; init; }
    public required string FromStationId { get; init; }
}