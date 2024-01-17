using FluentValidation;

using TrainAPI.Domain.Constants;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Extensions;

public static class ValidationExtensions
{
    public static void ValidateName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty()
            .WithMessage(SharedErrors<T>.MissingName.Description)
            .WithErrorCode(SharedErrors<T>.MissingName.Code)
            .Length(DomainConstants.MinNameLength, DomainConstants.MaxNameLength)
            .WithMessage(SharedErrors<T>.InvalidName.Description)
            .WithErrorCode(SharedErrors<T>.InvalidName.Code);
    }

    public static void ValidateFirstName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty()
            .WithMessage(SharedErrors<T>.MissingFirstName.Description)
            .WithErrorCode(SharedErrors<T>.MissingFirstName.Code)
            .Length(DomainConstants.MinNameLength, DomainConstants.MaxNameLength)
            .WithMessage(SharedErrors<T>.InvalidFirstName.Description)
            .WithErrorCode(SharedErrors<T>.InvalidFirstName.Code);
    }

    public static void ValidateLastName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty()
            .WithMessage(SharedErrors<T>.MissingLastName.Description)
            .WithErrorCode(SharedErrors<T>.MissingLastName.Code)
            .Length(DomainConstants.MinNameLength, DomainConstants.MaxNameLength)
            .WithMessage(SharedErrors<T>.InvalidLastName.Description)
            .WithErrorCode(SharedErrors<T>.InvalidLastName.Code);
    }

    public static void ValidateEmailAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty()
            .WithMessage(SharedErrors<T>.MissingEmailAddress.Description)
            .WithErrorCode(SharedErrors<T>.MissingEmailAddress.Code)
            .EmailAddress()
            .WithMessage(SharedErrors<T>.InvalidEmailAddress.Description)
            .WithErrorCode(SharedErrors<T>.InvalidEmailAddress.Code);
    }
}