using FluentValidation;

using TrainAPI.Application.Extensions;
using TrainAPI.Domain.Constants;

namespace TrainAPI.Application.Features.Auth.Register;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x)
            .NotEmpty();

        RuleFor(x => x.FirstName).ValidateFirstName();
        RuleFor(x => x.LastName).ValidateLastName();
        RuleFor(x => x.EmailAddress).ValidateEmailAddress();

        RuleFor(x => x.Role)
            .Must(x => Roles.AllRoles.Contains(x))
            .WithMessage("These are the valid roles: " + string.Join(", ", Roles.AllRoles));
    }
}