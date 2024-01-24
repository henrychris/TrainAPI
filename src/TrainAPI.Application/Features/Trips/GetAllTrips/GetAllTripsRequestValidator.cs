using FluentValidation;

using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Trips.GetAllTrips;

public class GetAllTripsRequestValidator : AbstractValidator<GetAllTripsRequest>
{
    public GetAllTripsRequestValidator()
    {
        RuleFor(x => x.FromStationId)
            .NotEmpty()
            .WithMessage(Errors.Trip.FromStationMissing.Description)
            .WithErrorCode(Errors.Trip.FromStationMissing.Code);

        RuleFor(x => x.ToStationId)
            .NotEmpty()
            .WithMessage(Errors.Trip.ToStationMissing.Description)
            .WithErrorCode(Errors.Trip.ToStationMissing.Code);
    }
}