using FluentValidation;

using TrainAPI.Application.Extensions;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Trips.CreateTrip;

public class CreateTripRequestValidator : AbstractValidator<CreateTripRequest>
{
    public CreateTripRequestValidator()
    {
        RuleFor(x => x)
            .NotEmpty();

        RuleFor(x => x.Name)
            .ValidateName();

        RuleFor(x => x.FromStationId)
            .NotEmpty()
            .WithMessage(Errors.Trip.FromStationMissing.Description)
            .WithErrorCode(Errors.Trip.FromStationMissing.Code)
            .NotEqual(request => request.ToStationId)
            .WithMessage(Errors.Trip.MatchingStations.Description)
            .WithErrorCode(Errors.Trip.MatchingStations.Code);

        RuleFor(x => x.ToStationId)
            .NotEmpty()
            .WithMessage(Errors.Trip.ToStationMissing.Description)
            .WithErrorCode(Errors.Trip.ToStationMissing.Code);

        RuleFor(x => x.TrainId)
            .NotEmpty()
            .WithMessage(Errors.Trip.TrainMissing.Description)
            .WithErrorCode(Errors.Trip.TrainMissing.Code);

        RuleFor(x => x.DistanceInKilometers)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage(Errors.Trip.MinimumDistance.Description)
            .WithErrorCode(Errors.Trip.MinimumDistance.Code);


        RuleFor(request => request.ArrivalTime)
            .GreaterThan(request => request.DateOfTrip)
            .WithMessage(Errors.Trip.ArrivalTimeBeforeDate.Description)
            .WithErrorCode(Errors.Trip.ArrivalTimeBeforeDate.Code);

        // todo: arrival time should be before departure time, as that is when the train gets to its destination

        RuleFor(request => request.DepartureTime)
            .GreaterThan(request => request.DateOfTrip)
            .WithMessage(Errors.Trip.DepartureTimeBeforeDate.Description)
            .WithErrorCode(Errors.Trip.DepartureTimeBeforeDate.Code)
            .LessThan(request => request.ArrivalTime)
            .WithMessage(Errors.Trip.DepartureTimeAfterArrivalTime.Description)
            .WithErrorCode(Errors.Trip.DepartureTimeAfterArrivalTime.Code);

    }
}