using FluentValidation;

using TrainAPI.Application.Extensions;

namespace TrainAPI.Application.Features.Trains.UpdateTrain;

public class UpdateTrainRequestValidator : AbstractValidator<UpdateTrainRequest>
{
    public UpdateTrainRequestValidator()
    {
        RuleFor(x => x.TrainId)
            .NotEmpty();

        When(x => x.Name is not null, () =>
            RuleFor(x => x.Name)!
                .ValidateName());

        When(x => x.Code is not null, () =>
            RuleFor(x => x.Code)!
                .ValidateStationCode());
    }
}