using TrainAPI.Domain.Entities;

namespace TrainAPI.Application.Contracts;

public interface IBookingService
{
    Task CreateBooking(Booking booking);
    Task<Booking?> GetBookingAsync(string bookingId);
    Task<bool> IsSeatBooked(int seatNo, string coachId);
    Task TemporarilyReserveSeats(List<Passenger> passengers, string bookingId);
    Task UpdateBookingAsync(Booking booking);

}