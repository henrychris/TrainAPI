using TrainAPI.Application.Contracts;
using TrainAPI.Domain.Entities;
using TrainAPI.Infrastructure.Data;

namespace TrainAPI.Infrastructure.Services;

public class BookingService(DataContext context) : IBookingService
{
	public async Task CreateBooking(Booking booking)
	{
		await context.Bookings.AddAsync(booking);
		await context.SaveChangesAsync();
	}

	public async Task<bool> IsSeatAvailable(int seatNo, string coachId)
	{
		var coach = await context.Coaches.FindAsync(coachId);
		if (coach is null)
		{
			return false;
		}

		var seat = coach.Seats.FirstOrDefault(s => s.SeatNumber == seatNo);
		if (seat is null)
		{
			return false;
		}

		return seat.IsBooked;
	}

	/// <summary>
	///    Temporarily reserves the seats for the passengers.
	///    This function does not throw any exceptions if the seat is not available.
	///    It just ignores the seat. This is because the validation is done before calling this function.
	/// </summary>
	/// <param name="passengers"></param>
	/// <returns></returns>
	public async Task TemporarilyReserveSeats(List<Passenger> passengers)
	{
		foreach (var passenger in passengers)
		{
			var coach = await context.Coaches.FindAsync(passenger.CoachId);
			if (coach is null)
			{
				continue;
			}

			var seat = coach.Seats.FirstOrDefault(s => s.SeatNumber == passenger.SeatNumber);
			if (seat is null)
			{
				continue;
			}

			seat.IsBooked = true;
			context.Coaches.Update(coach);

			// todo: add a hangfire job to unreserve the seat after 10 minutes
		}

		await context.SaveChangesAsync();
	}
}