using ErrorOr;

using FluentValidation;

using MediatR;

using TrainAPI.Application.Contracts;
using TrainAPI.Application.Extensions;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Trains.UpdateTrain;

public class UpdateTrainRequestDto
{
    public string? Name { get; set; }
    public string? Code { get; set; }
}

public class UpdateTrainRequest : IRequest<ErrorOr<Updated>>
{
    public required string TrainId { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
}

public class UpdateTrainRequestHandler(
    ITrainService trainService,
    IValidator<UpdateTrainRequest> validator,
    ILogger<UpdateTrainRequestHandler> logger) : IRequestHandler<UpdateTrainRequest, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateTrainRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Trying to update a train. Request: {request}", request);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.ToErrorList();
            logger.LogError("Validation failed for {request}. Errors: {errors}", nameof(UpdateTrainRequest),
                errors);
            return errors;
        }

        var train = await trainService.GetTrain(request.TrainId);
        if (train is null)
        {
            logger.LogError("Train not found. ID: {id}", request.TrainId);
            return SharedErrors<Train>.NotFound;
        }

        train.Name = request.Name ?? train.Name;
        train.Code = request.Code ?? train.Code;

        await trainService.UpdateTrain(train);
        logger.LogInformation("Train {id} has been updated.", train.Id);
        return Result.Updated;
    }
}