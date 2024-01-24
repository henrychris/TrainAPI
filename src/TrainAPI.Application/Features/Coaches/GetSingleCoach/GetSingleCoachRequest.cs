using ErrorOr;

using MediatR;

using TrainAPI.Application.Contracts;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Coaches.GetSingleCoach;

public class GetSingleCoachRequest : IRequest<ErrorOr<GetCoachResponse>>
{
    public string CoachId { get; set; } = string.Empty;
}

public class GetSingleCoachRequestHandler(
    ICoachService coachService,
    ILogger<GetSingleCoachRequestHandler> logger) : IRequestHandler<GetSingleCoachRequest, ErrorOr<GetCoachResponse>>
{
    public async Task<ErrorOr<GetCoachResponse>> Handle(GetSingleCoachRequest request,
                                                        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching coach with Id: {id}.", request.CoachId);

        var coach = await coachService.GetCoach(request.CoachId);
        if (coach is null)
        {
            logger.LogError("Coach not found. Id: {id}.", request.CoachId);
            return SharedErrors<Coach>.NotFound;
        }

        logger.LogInformation("Successfully fetched Coach with ID: {id}", request.CoachId);
        return CoachMapper.ToGetCoachResponse(coach);
    }
}