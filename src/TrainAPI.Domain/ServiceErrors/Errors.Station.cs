using ErrorOr;

using TrainAPI.Domain.Constants;

namespace TrainAPI.Domain.ServiceErrors;

public static partial class Errors
{
    public static class Station
    {
        public static Error MissingCode => Error.Unauthorized(
            code: "Station.MissingCode",
            description: "No code was provided.");

        public static Error InvalidCode => Error.Unauthorized(
            code: "Station.InvalidCode",
            description: $"The code should be {DomainConstants.MAX_STATION_CODE_LENGTH} characters long.");
    }
}