using TrainAPI.Domain.Entities;

namespace TrainAPI.Application.Features.Coaches.GetSingleCoach;

public class GetCoachResponse
{
    public required string Id { get; set; }

    public required string Name { get; set; }
    public required string Class { get; set; }
    public required int SeatCount { get; set; }
    public required int AvailableSeats { get; set; }

    public required List<TravellerPairs> TravellerCategories { get; set; }
}