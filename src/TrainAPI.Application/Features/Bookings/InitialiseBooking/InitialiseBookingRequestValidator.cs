using FluentValidation;

using TrainAPI.Application.Extensions;

using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Bookings.InitialiseBooking;

public class InitialiseBookingRequestValidator : AbstractValidator<InitialiseBookingRequest>
{
    public InitialiseBookingRequestValidator()
    {
        RuleFor(x => x.TripId).NotEmpty()
        .WithMessage(Errors.Bookings.TripIdIsRequired.Description)
        .WithErrorCode(Errors.Bookings.TripIdIsRequired.Code);

        RuleFor(x => x.Name).ValidateName();

        RuleFor(x => x.ContactInfo)
        .NotEmpty()
        .WithMessage(Errors.Bookings.ContactInfoRequired.Description)
        .WithErrorCode(Errors.Bookings.ContactInfoRequired.Code);

        RuleFor(request => request.ContactInfo).SetValidator(new ContactDetailsValidator());

        RuleFor(request => request.Passengers).NotEmpty()
        .WithMessage(Errors.Bookings.PassengersListCannotBeEmpty.Description)
        .WithErrorCode(Errors.Bookings.PassengersListCannotBeEmpty.Code);

        RuleForEach(request => request.Passengers).SetValidator(new PassengerValidator());
    }
}