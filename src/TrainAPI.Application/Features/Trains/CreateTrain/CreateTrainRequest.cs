using MediatR;

using ErrorOr;

using FluentValidation;

using TrainAPI.Application.Contracts;
using TrainAPI.Application.Extensions;

namespace TrainAPI.Application.Features.Trains.CreateTrain;

public class CreateTrainRequest : IRequest<ErrorOr<CreateTrainResponse>>
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}

public class CreateTrainRequestHandler(
    ITrainService trainService,
    ILogger<CreateTrainRequestHandler> logger,
    IValidator<CreateTrainRequest> validator) : IRequestHandler<CreateTrainRequest, ErrorOr<CreateTrainResponse>>
{
    public async Task<ErrorOr<CreateTrainResponse>> Handle(CreateTrainRequest request,
                                                           CancellationToken cancellationToken)
    {
        logger.LogInformation("Trying to create a new train. Request: {request}", request);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.ToErrorList();
            logger.LogError("Validation failed for {request}. Errors: {errors}", nameof(CreateTrainRequest),
                errors);
            return errors;
        }

        var train = TrainMapper.CreateTrain(request);
        await trainService.CreateTrain(train);

        logger.LogInformation("Train Created. Id: {id}", train.Id);
        return TrainMapper.ToCreateTrainResponse(train);
    }
}