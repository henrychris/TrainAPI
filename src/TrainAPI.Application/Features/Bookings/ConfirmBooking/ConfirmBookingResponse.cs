using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainAPI.Application.Features.Bookings.ConfirmBooking
{
    public class ConfirmBookingResponse
    {
        public required string BookingId { get; set; }
    }
}