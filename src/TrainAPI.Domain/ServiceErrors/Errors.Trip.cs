using ErrorOr;

namespace TrainAPI.Domain.ServiceErrors;

public static partial class Errors
{
    public static class Trip
    {
        public static Error ClashingTrips => Error.Validation(
            code: "Station.ClashingTrips",
            description:
            "Some trips are clashing. It's possible that another trip was scheduled at the same time and location as this one.");

        public static Error FromStationDoesNotExist => Error.Validation(
            code: "Station.FromStationDoesNotExist",
            description:
            "The 'from' station does not exist.");

        public static Error ToStationDoesNotExist => Error.Validation(
            code: "Station.ToStationDoesNotExist",
            description:
            "The 'to' station does not exist.");

        public static Error ToStationMissing => Error.Validation(
            code: "Station.ToStationMissing",
            description:
            "There must be a destination station.");

        public static Error FromStationMissing => Error.Validation(
            code: "Station.FromStationMissing",
            description:
            "There train must be leaving a station.");

        public static Error TrainMissing => Error.Validation(
            code: "Station.TrainMissing",
            description:
            "There is no train selected for this trip.");

        public static Error MinimumDistance => Error.Validation(
            code: "Station.MinimumDistance",
            description:
            "A trip must be at least 0 kilometers.");

        public static Error MatchingStations => Error.Validation(
            code: "Station.MatchingStations",
            description:
            "The two stations must not be different. You can come from and go to the same place.");

        public static Error ArrivalTimeBeforeDate => Error.Validation(
            code: "Trip.ArrivalTimeBeforeDate",
            description: "ArrivalTime must be after DateOfTrip.");

        public static Error DepartureTimeBeforeDate => Error.Validation(
            code: "Trip.DepartureTimeBeforeDate",
            description: "DepartureTime must be after DateOfTrip.");

        public static Error DepartureTimeBeforeArrivalTime => Error.Validation(
            code: "Trip.DepartureTimeBeforeArrivalTime",
            description: "DepartureTime must be after ArrivalTime.");

        public static Error TripAlreadyDeparted => Error.Validation(
            code: "Trip.TripAlreadyDeparted",
            description: "The train has already departed.");
    }
}