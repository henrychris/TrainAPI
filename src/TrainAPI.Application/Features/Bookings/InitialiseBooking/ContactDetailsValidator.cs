using FluentValidation;

using TrainAPI.Application.Extensions;

using TrainAPI.Domain.Entities;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Bookings.InitialiseBooking
{
    public class ContactDetailsValidator : AbstractValidator<ContactDetails>
    {
        public ContactDetailsValidator()
        {
            RuleFor(contact => contact.EmailAddress).ValidateEmailAddress();
            RuleFor(contact => contact.PhoneNumber)
                .NotEmpty()
                .WithMessage(Errors.Bookings.ContactPhoneNumberIsRequired.Description)
                .WithErrorCode(Errors.Bookings.ContactPhoneNumberIsRequired.Code);
        }
    }
}