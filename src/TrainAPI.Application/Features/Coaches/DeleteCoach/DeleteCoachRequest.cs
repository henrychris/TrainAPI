using ErrorOr;

using MediatR;

using TrainAPI.Application.Contracts;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Coaches.DeleteCoach;

public class DeleteCoachRequest : IRequest<ErrorOr<Deleted>>
{
    public string CoachId { get; set; } = string.Empty;
}

public class DeleteCoachRequestHandler(ICoachService coachService, ILogger<DeleteCoachRequestHandler> logger)
    : IRequestHandler<DeleteCoachRequest, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteCoachRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Received request to delete coach. ID: {id}", request.CoachId);

        var coach = await coachService.GetCoach(request.CoachId);
        if (coach is null)
        {
            logger.LogError("Coach not found. ID: {id}", request.CoachId);
            return SharedErrors<Coach>.NotFound;
        }

        await coachService.DeleteCoach(coach);
        logger.LogInformation("Successfully deleted Coach with ID: {id}", request.CoachId);
        return Result.Deleted;
    }
}