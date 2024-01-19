using FluentValidation;

using TrainAPI.Application.Extensions;

namespace TrainAPI.Application.Features.Trains.CreateTrain;

public class CreateTrainRequestValidator : AbstractValidator<CreateTrainRequest>
{
    public CreateTrainRequestValidator()
    {
        RuleFor(x => x)
            .NotEmpty();

        RuleFor(x => x.Name)
            .ValidateName();

        RuleFor(x => x.Code)
            .ValidateStationCode();
    }
}