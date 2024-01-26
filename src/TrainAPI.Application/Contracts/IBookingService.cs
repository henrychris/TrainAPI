using TrainAPI.Domain.Entities;

namespace TrainAPI.Application.Contracts;

public interface IBookingService
{
    Task CreateBooking(Booking booking);
    Task<bool> IsSeatAvailable(int seatNo, string coachId);
    Task TemporarilyReserveSeats(List<Passenger> passengers, string bookingId);
}