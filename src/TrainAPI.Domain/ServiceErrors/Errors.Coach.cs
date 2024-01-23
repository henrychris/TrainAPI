using ErrorOr;

namespace TrainAPI.Domain.ServiceErrors;

public static partial class Errors
{
    public static class Coach
    {
        public static Error NameMissing => Error.Validation("Coach.NameMissing", "Name is required.");

        public static Error InvalidClass =>
            Error.Validation("Coach.InvalidClass", "Class must be 'business', 'first', or 'regular'.");

        public static Error InvalidSeatCount =>
            Error.Validation("Coach.InvalidSeatCount", "Seat count must be at least 0.");

        public static Error InvalidAvailableSeats =>
            Error.Validation("Coach.InvalidAvailableSeats", "Available seats must be at least 0.");

        public static Error InvalidTravellerPairs => Error.Validation("Coach.InvalidTravellerPairs",
            "There must be at least one traveller of type 'Child' and one of type 'Adult'.");

        public static Error InvalidTravellerType =>
            Error.Validation("Coach.InvalidTravellerType", "Traveller type must be 'Child' or 'Adult'.");

        public static Error InvalidTravellerPrice =>
            Error.Validation("Coach.InvalidTravellerPrice", "Traveller price must be at least 0.");

        public static Error InvalidTravellerPairsCount => Error.Validation("Coach.InvalidTravellerPairsCount",
            "A TravellerPairs should have at most one child and one adult.");
    }
}