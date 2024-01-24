using FluentValidation;

using TrainAPI.Application.Extensions;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Trips.UpdateTrip;

public class UpdateTripRequestValidator : AbstractValidator<UpdateTripRequest>
{
    public UpdateTripRequestValidator()
    {
        RuleFor(x => x.TripId)
            .NotEmpty();

        When(x => x.Name is not null, () =>
        {
            RuleFor(x => x.Name)
                !.ValidateName();
        });

        When(x => x.FromStationId is not null, () =>
        {
            RuleFor(x => x.FromStationId)
                !.NotEmpty()
                 .WithMessage(Errors.Trip.FromStationMissing.Description)
                 .WithErrorCode(Errors.Trip.FromStationMissing.Code)
                 .NotEqual(request => request.ToStationId)
                 .WithMessage(Errors.Trip.MatchingStations.Description)
                 .WithErrorCode(Errors.Trip.MatchingStations.Code);
        });

        When(x => x.ToStationId is not null, () =>
        {
            RuleFor(x => x.ToStationId)
                !.NotEmpty()
                 .WithMessage(Errors.Trip.ToStationMissing.Description)
                 .WithErrorCode(Errors.Trip.ToStationMissing.Code);
        });

        When(x => x.TrainId is not null, () =>
        {
            RuleFor(x => x.TrainId)
                !.NotEmpty()
                 .WithMessage(Errors.Trip.TrainMissing.Description)
                 .WithErrorCode(Errors.Trip.TrainMissing.Code);
        });

        When(x => x.DistanceInKilometers is not null, () =>
        {
            RuleFor(x => x.DistanceInKilometers)
                !.NotEmpty()
                 .GreaterThan(0)
                 .WithMessage(Errors.Trip.MinimumDistance.Description)
                 .WithErrorCode(Errors.Trip.MinimumDistance.Code);
        });

        When(x => x.ArrivalTime is not null, () =>
        {
            RuleFor(x => x.ArrivalTime)
                !.GreaterThan(request => request.DateOfTrip)
                 .WithMessage(Errors.Trip.ArrivalTimeBeforeDate.Description)
                 .WithErrorCode(Errors.Trip.ArrivalTimeBeforeDate.Code);
        });

        When(x => x.DepartureTime is not null, () =>
        {
            RuleFor(x => x.DepartureTime)
                !.GreaterThan(request => request.DateOfTrip)
                 .WithMessage(Errors.Trip.DepartureTimeBeforeDate.Description)
                 .WithErrorCode(Errors.Trip.DepartureTimeBeforeDate.Code)
                 .GreaterThan(request => request.ArrivalTime)
                 .WithMessage(Errors.Trip.DepartureTimeBeforeArrivalTime.Description)
                 .WithErrorCode(Errors.Trip.DepartureTimeBeforeArrivalTime.Code);
        });
    }
}