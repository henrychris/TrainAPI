using FluentValidation;

using TrainAPI.Application.Extensions;

namespace TrainAPI.Application.Features.Stations.UpdateStation;

public class UpdateStationRequestValidator : AbstractValidator<UpdateStationRequest>
{
    public UpdateStationRequestValidator()
    {
        RuleFor(x => x.StationId)
            .NotEmpty();

        When(x => x.Name is not null, () =>
            RuleFor(x => x.Name)!
                .ValidateName());

        When(x => x.Code is not null, () =>
            RuleFor(x => x.Code)!
                .ValidateStationCode());
    }
}