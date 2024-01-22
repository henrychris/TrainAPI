using ErrorOr;

using FluentValidation;

using MediatR;

using TrainAPI.Application.Contracts;
using TrainAPI.Application.Extensions;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Stations.UpdateStation;

public class UpdateStationRequestDto
{
    public string? Name { get; set; }
    public string? Code { get; set; }
}

public class UpdateStationRequest : IRequest<ErrorOr<Updated>>
{
    public required string StationId { get; init; }
    public string? Name { get; init; }
    public string? Code { get; init; }
}

public class UpdateStationRequestHandler(
    IStationService stationService,
    IValidator<UpdateStationRequest> validator,
    ILogger<UpdateStationRequestHandler> logger) : IRequestHandler<UpdateStationRequest, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateStationRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Trying to update a station. Request: {request}", request);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.ToErrorList();
            logger.LogError("Validation failed for {request}. Errors: {errors}", nameof(UpdateStationRequest),
                errors);
            return errors;
        }

        var station = await stationService.GetStation(request.StationId);
        if (station is null)
        {
            logger.LogError("Station not found. ID: {id}", request.StationId);
            return SharedErrors<Station>.NotFound;
        }

        station.Name = request.Name ?? station.Name;
        station.Code = request.Code ?? station.Code;

        await stationService.UpdateStation(station);
        logger.LogInformation("Station {id} has been updated.", station.Id);
        return Result.Updated;
    }
}