using FluentValidation;

using TrainAPI.Domain.Entities;
using TrainAPI.Domain.Enums;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Coaches.CreateCoach;

public class TravellerPairsValidator : AbstractValidator<TravellerPairs>
{
    public TravellerPairsValidator()
    {
        RuleFor(x => x.Type)
            .Must(type => type.Equals(TravellerCategory.Child.ToString(), StringComparison.CurrentCultureIgnoreCase) ||
                          type.Equals(TravellerCategory.Adult.ToString(), StringComparison.CurrentCultureIgnoreCase))
            .WithMessage(Errors.Coach.InvalidTravellerType.Description)
            .WithErrorCode(Errors.Coach.InvalidTravellerType.Code);

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage(Errors.Coach.InvalidTravellerPrice.Description)
            .WithErrorCode(Errors.Coach.InvalidTravellerPrice.Code);
    }
}