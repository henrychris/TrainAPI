using ErrorOr;

using FluentValidation;

using MediatR;

using TrainAPI.Application.Contracts;
using TrainAPI.Application.Extensions;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Coaches.UpdateCoach;

public class UpdateCoachRequestDto
{
    public string? Name { get; set; }
    public string? Class { get; set; }
    public int? SeatCount { get; set; }
    public int? AvailableSeats { get; set; }

    public List<TravellerPairs>? TravellerCategories { get; set; }
    public string? TrainId { get; set; }
}

public class UpdateCoachRequest : IRequest<ErrorOr<Updated>>
{
    public required string CoachId { get; init; }
    public string? Name { get; init; }
    public string? Class { get; init; }
    public int? SeatCount { get; init; }
    public int? AvailableSeats { get; init; }

    public List<TravellerPairs>? TravellerCategories { get; init; }
    public string? TrainId { get; init; }
}

public class UpdateCoachRequestHandler(
    ICoachService coachService,
    ITrainService trainService,
    IValidator<UpdateCoachRequest> validator,
    ILogger<UpdateCoachRequestHandler> logger) : IRequestHandler<UpdateCoachRequest, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateCoachRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Trying to update a coach. Request: {request}", request);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.ToErrorList();
            logger.LogError("Validation failed for {request}. Errors: {errors}", nameof(UpdateCoachRequest),
                errors);
            return errors;
        }

        var coach = await coachService.GetCoach(request.CoachId);
        if (coach is null)
        {
            logger.LogError("Coach not found. ID: {id}", request.CoachId);
            return SharedErrors<Coach>.NotFound;
        }

        if (request.TrainId is not null)
        {
            var train = await trainService.GetTrain(request.TrainId);
            if (train is null)
            {
                logger.LogError("Train not found. ID: {id}", request.TrainId);
                return SharedErrors<Train>.NotFound;
            }
        }

        coach.Name = request.Name ?? coach.Name;
        coach.Class = request.Class ?? coach.Class;
        coach.SeatCount = request.SeatCount ?? coach.SeatCount;
        coach.AvailableSeats = request.AvailableSeats ?? coach.AvailableSeats;
        coach.TravellerCategories = request.TravellerCategories ?? coach.TravellerCategories;
        coach.TrainId = request.TrainId ?? coach.TrainId;

        await coachService.UpdateCoach(coach);
        logger.LogInformation("Coach {id} has been updated.", coach.Id);
        return Result.Updated;
    }
}