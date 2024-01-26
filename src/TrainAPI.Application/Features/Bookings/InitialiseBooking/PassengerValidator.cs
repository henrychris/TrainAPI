using FluentValidation;

using TrainAPI.Domain.ServiceErrors;
using TrainAPI.Application.Extensions;
using TrainAPI.Domain.Entities;

namespace TrainAPI.Application.Features.Bookings.InitialiseBooking
{
    public class PassengerValidator : AbstractValidator<Passenger>
    {
        public PassengerValidator()
        {
            RuleFor(passenger => passenger.FullName)
            .NotEmpty()
            .WithMessage(Errors.Bookings.PassengerFullNameIsRequired.Description)
            .WithErrorCode(Errors.Bookings.PassengerFullNameIsRequired.Code);

            RuleFor(passenger => passenger.EmailAddress).ValidateEmailAddress();

            RuleFor(passenger => passenger.PhoneNumber)
                .NotEmpty()
                .WithMessage(Errors.Bookings.PassengerFullNameIsRequired.Description)
                .WithErrorCode(Errors.Bookings.PassengerFullNameIsRequired.Code);

            RuleFor(passenger => passenger.CoachId)
                .NotEmpty()
                .WithMessage(Errors.Bookings.PassengerCoachIdIsRequired.Description)
                .WithErrorCode(Errors.Bookings.PassengerCoachIdIsRequired.Code);

            RuleFor(passenger => passenger.SeatNumber)
                .GreaterThan(0)
                .WithMessage(Errors.Bookings.PassengerSeatNumberInvalid.Description)
                .WithErrorCode(Errors.Bookings.PassengerSeatNumberInvalid.Code);
        }
    }
}