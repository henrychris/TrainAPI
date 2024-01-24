using ErrorOr;

using MediatR;

using TrainAPI.Application.Contracts;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Trains.DeleteTrain;

public class DeleteTrainRequest : IRequest<ErrorOr<Deleted>>
{
    public string TrainId { get; set; } = string.Empty;
}

public class DeleteTrainRequestHandler(ITrainService trainService, ILogger<DeleteTrainRequestHandler> logger)
    : IRequestHandler<DeleteTrainRequest, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteTrainRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Received request to delete train. ID: {id}", request.TrainId);

        var train = await trainService.GetTrain(request.TrainId);
        if (train is null)
        {
            logger.LogError("Train not found. ID: {id}", request.TrainId);
            return SharedErrors<Train>.NotFound;
        }

        await trainService.DeleteTrain(train);
        logger.LogInformation("Successfully deleted Train with ID: {id}", request.TrainId);
        return Result.Deleted;
    }
}