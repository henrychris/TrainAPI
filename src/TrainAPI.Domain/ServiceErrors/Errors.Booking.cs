using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ErrorOr;

namespace TrainAPI.Domain.ServiceErrors
{
    public static partial class Errors
    {
        public class Booking
        {
            public static Error BookingAlreadyConfirmed => Error.Validation("BookingAlreadyConfirmed", "The booking has already been confirmed.");

            public static Error BookingAlreadyExpired => Error.Validation("BookingAlreadyExpired", "The booking has already expired.");
        }
    }
}