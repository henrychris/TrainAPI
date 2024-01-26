using ErrorOr;

namespace TrainAPI.Domain.ServiceErrors
{
    public static partial class Errors
    {
        public static class Bookings
        {
            public static Error TripIdIsRequired => Error.Validation(
                code: "Booking.TripIdRequired",
                description: "TripId is required.");

            public static Error ContactInfoRequired => Error.Validation(
                code: "Booking.ContactInfoRequired",
                description: "Contact details are required.");

            public static Error ContactPhoneNumberIsRequired => Error.Validation(
                code: "Booking.ContactPhoneNumberRequired",
                description: "PhoneNumber in contact details is required.");

            public static Error PassengersListCannotBeEmpty => Error.Validation(
                code: "Booking.PassengersListEmpty",
                description: "Passengers list must not be empty.");

            public static Error PassengerFullNameIsRequired => Error.Validation(
                code: "Booking.PassengerFullNameRequired",
                description: "The passengers full name is required.");

            public static Error PassengerCoachIdIsRequired => Error.Validation(
                code: "Booking.PassengerCoachIdRequired",
                description: "CoachId for a passenger is required.");

            public static Error PassengerSeatNumberInvalid => Error.Validation(
                code: "Booking.PassengerSeatNumberInvalid",
                description: "SeatNumber for a passenger must be greater than 0.");
        }
    }
}