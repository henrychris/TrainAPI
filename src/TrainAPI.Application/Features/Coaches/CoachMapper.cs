using MediatR;

using TrainAPI.Application.Features.Coaches.CreateCoach;
using TrainAPI.Application.Features.Coaches.GetSingleCoach;
using TrainAPI.Application.Features.Coaches.UpdateCoach;
using TrainAPI.Domain.Entities;

namespace TrainAPI.Application.Features.Coaches;

public static class CoachMapper
{
    public static Coach CreateCoach(CreateCoachRequest request)
    {
        var coach =  new Coach
        {
            Name = request.Name,
            Class = request.Class,
            SeatCount = request.SeatCount,
            TravellerCategories = request.TravellerCategories,
            TrainId = request.TrainId
        };
        
        coach.InitializeSeats(request.SeatCount);
        return coach;
    }

    public static CreateCoachResponse ToCreateCoachResponse(Coach coach)
    {
        return new CreateCoachResponse { CoachId = coach.Id };
    }

    public static GetCoachResponse ToGetCoachResponse(Coach coach)
    {
        return new GetCoachResponse
        {
            Id = coach.Id,
            Name = coach.Name,
            Class = coach.Class,
            SeatCount = coach.SeatCount,
            AvailableSeats = coach.AvailableSeats,
            Seats = coach.Seats,
            TravellerCategories = coach.TravellerCategories
        };
    }

    public static UpdateCoachRequest CreateUpdateCoachRequest(string id, UpdateCoachRequestDto request)
    {
        return new UpdateCoachRequest
        {
            CoachId = id,
            Name = request.Name,
            Class = request.Class,
            SeatCount = request.SeatCount,
            TravellerCategories = request.TravellerCategories,
            TrainId = request.TrainId
        };
    }
}