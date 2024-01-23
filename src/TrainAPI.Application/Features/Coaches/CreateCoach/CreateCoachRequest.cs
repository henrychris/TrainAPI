using ErrorOr;

using FluentValidation;

using MediatR;

using TrainAPI.Application.Contracts;
using TrainAPI.Application.Extensions;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Coaches.CreateCoach;

public class CreateCoachRequest : IRequest<ErrorOr<CreateCoachResponse>>
{
    public string Name { get; set; } = string.Empty;
    public string Class { get; set; } = string.Empty;
    public int SeatCount { get; set; }
    public int AvailableSeats { get; set; }
    public List<TravellerPairs> TravellerCategories { get; set; } = [];
    public string TrainId { get; set; } = string.Empty;
}

public class CreateCoachRequestHandler(
    ICoachService coachService,
    ITrainService trainService,
    IValidator<CreateCoachRequest> validator,
    ILogger<CreateCoachRequestHandler> logger) : IRequestHandler<CreateCoachRequest, ErrorOr<CreateCoachResponse>>
{
    public async Task<ErrorOr<CreateCoachResponse>> Handle(CreateCoachRequest request,
                                                           CancellationToken cancellationToken)
    {
        logger.LogInformation("Trying to create a new coach. Request: {request}", request);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.ToErrorList();
            logger.LogError("Validation failed for {request}. Errors: {errors}", nameof(CreateCoachRequest),
                errors);
            return errors;
        }

        if (await trainService.GetTrain(request.TrainId) is null)
        {
            logger.LogError("The selected train does not exist. Id: {id}", request.TrainId);
            return SharedErrors<Train>.NotFound;
        }

        Coach coach = CoachMapper.CreateCoach(request);
        await coachService.CreateCoach(coach);

        logger.LogInformation("Coach created. Id: {id}.", coach.Id);
        return CoachMapper.ToCreateCoachResponse(coach);
    }
}