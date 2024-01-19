using ErrorOr;

using FluentValidation;

using MediatR;

using TrainAPI.Application.Contracts;
using TrainAPI.Application.Extensions;
using TrainAPI.Domain.Entities;

namespace TrainAPI.Application.Features.Stations.CreateStation;

public class CreateStationRequest : IRequest<ErrorOr<CreateStationResponse>>
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}

public class CreateStationRequestHandler(
    IStationService stationService,
    ILogger<CreateStationRequestHandler> logger,
    IValidator<CreateStationRequest> validator) : IRequestHandler<CreateStationRequest, ErrorOr<CreateStationResponse>>
{
    public async Task<ErrorOr<CreateStationResponse>> Handle(CreateStationRequest request,
                                                             CancellationToken cancellationToken)
    {
        logger.LogInformation("Trying to create a new station. Request: {request}", request);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.ToErrorList();
            logger.LogInformation("Validation failed for {request}. Errors: {errors}", nameof(CreateStationRequest),
                errors);
            return errors;
        }

        Station station = StationMapper.CreateStation(request);
        await stationService.CreateStation(station);

        logger.LogInformation("Station Created. Id: {id}", station.Id);
        return StationMapper.ToCreateStationResponse(station);
    }
}