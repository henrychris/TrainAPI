using FluentValidation;

using TrainAPI.Application.Extensions;

namespace TrainAPI.Application.Features.Stations.CreateStation;

public class CreateStationRequestValidator : AbstractValidator<CreateStationRequest>
{
    public CreateStationRequestValidator()
    {
        RuleFor(x => x)
            .NotEmpty();

        RuleFor(x => x.Name)
            .ValidateName();

        RuleFor(x => x.Code)
            .ValidateStationCode();
    }
}