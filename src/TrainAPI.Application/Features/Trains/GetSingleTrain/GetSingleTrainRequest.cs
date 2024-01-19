using ErrorOr;

using MediatR;

using TrainAPI.Application.Contracts;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Trains.GetSingleTrain;

public class GetSingleTrainRequest : IRequest<ErrorOr<GetTrainResponse>>
{
    public string TrainId { get; set; } = string.Empty;
}

public class GetSingleTrainRequestHandler(
    ITrainService trainService,
    ILogger<GetSingleTrainRequestHandler> logger)
    : IRequestHandler<GetSingleTrainRequest, ErrorOr<GetTrainResponse>>
{
    public async Task<ErrorOr<GetTrainResponse>> Handle(GetSingleTrainRequest request,
                                                        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching train with Id: {id}.", request.TrainId);

        var train = await trainService.GetTrain(request.TrainId);
        if (train is null)
        {
            logger.LogError("Train not found. Id: {id}.", request.TrainId);
            return SharedErrors<Train>.NotFound;
        }

        logger.LogInformation("Successfully fetched Train with ID: {id}", request.TrainId);
        return TrainMapper.ToGetTrainResponse(train);
    }
}