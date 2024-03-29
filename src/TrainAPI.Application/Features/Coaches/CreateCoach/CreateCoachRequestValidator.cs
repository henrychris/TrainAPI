using FluentValidation;

using TrainAPI.Application.Extensions;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.Enums;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Application.Features.Coaches.CreateCoach;

public class CreateCoachRequestValidator : AbstractValidator<CreateCoachRequest>
{
    public CreateCoachRequestValidator()
    {
        RuleFor(x => x)
            .NotEmpty();

        RuleFor(x => x.Name)
            .ValidateName();

        RuleFor(x => x.Class)
            .Must(BeAValidClass)
            .WithMessage(Errors.Coach.InvalidClass.Description)
            .WithErrorCode(Errors.Coach.InvalidClass.Code);

        RuleFor(x => x.SeatCount)
            .GreaterThanOrEqualTo(0)
            .WithMessage(Errors.Coach.InvalidSeatCount.Description)
            .WithErrorCode(Errors.Coach.InvalidSeatCount.Code);

        RuleFor(x => x.TravellerCategories)
            .Must(HaveValidTravellerPairs)
            .WithMessage(Errors.Coach.InvalidTravellerPairs.Description)
            .WithErrorCode(Errors.Coach.InvalidTravellerPairs.Code)
            .Must(HaveAtMostOneChildAndOneAdult)
            .WithMessage(Errors.Coach.InvalidTravellerPairsCount.Description)
            .WithErrorCode(Errors.Coach.InvalidTravellerPairsCount.Code);
    }

    private static bool BeAValidClass(string className)
    {
        return className.Equals(CoachClass.Business.ToString(), StringComparison.CurrentCultureIgnoreCase) ||
               className.Equals(CoachClass.First.ToString(), StringComparison.CurrentCultureIgnoreCase) ||
               className.Equals(CoachClass.Regular.ToString(), StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool HaveValidTravellerPairs(List<TravellerPairs> travellerPairs)
    {
        var hasChild = travellerPairs.Any(pair =>
            pair.Type.Equals(TravellerCategory.Child.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            pair.Price >= 0);
        var hasAdult = travellerPairs.Any(pair =>
            pair.Type.Equals(TravellerCategory.Adult.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            pair.Price >= 0);

        return hasChild && hasAdult;
    }

    private static bool HaveAtMostOneChildAndOneAdult(List<TravellerPairs> travellerPairs)
    {
        var numberOfChild =
            travellerPairs.Count(pair =>
                pair.Type.Equals(TravellerCategory.Child.ToString(), StringComparison.CurrentCultureIgnoreCase));
        var numberOfAdult =
            travellerPairs.Count(pair =>
                pair.Type.Equals(TravellerCategory.Adult.ToString(), StringComparison.CurrentCultureIgnoreCase));
        return numberOfChild <= 1 && numberOfAdult <= 1;
    }
}