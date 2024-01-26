using Hangfire;

using TrainAPI.Application.Contracts;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.Entities.Other;
using TrainAPI.Domain.Enums;
using TrainAPI.Infrastructure.Data;

namespace TrainAPI.Infrastructure.Services;

public class BookingService(DataContext context, ILogger<BookingService> logger) : IBookingService
{
	// todo: move this to IOptions
	private const int MINUTES_TILL_UNRESERVE_SEATS = 10;
	public async Task CreateBooking(Booking booking)
	{
		await context.Bookings.AddAsync(booking);
		await context.SaveChangesAsync();
	}

	public async Task<bool> IsSeatBooked(int seatNo, string coachId)
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
	/// <param name="bookingId"></param>
	/// <returns></returns>
	public async Task TemporarilyReserveSeats(List<Passenger> passengers, string bookingId)
	{
		var coachAndSeatPairs = new List<SeatCoachIdPair>();
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

			coachAndSeatPairs.Add(new SeatCoachIdPair
			{
				CoachId = coach.Id,
				Seat = seat
			});
			logger.LogInformation("Reserved seat {seatNo} in coach {coachId} for booking {bookingId}",
				seat.SeatNumber, coach.Id, bookingId);
		}

		var jobId = BackgroundJob.Schedule(() => UnreserveSeat(new UnreserveSeatRequest
		{
			BookingId = bookingId,
			Seats = coachAndSeatPairs
		}), TimeSpan.FromMinutes(MINUTES_TILL_UNRESERVE_SEATS));

		logger.LogInformation("Scheduled a job to unreserve seats for booking {id} in {minutes} minutes", bookingId, MINUTES_TILL_UNRESERVE_SEATS);

		// we need to store the job id somewhere so that we can delete it if the payment fails.
		// we can store it in the booking table.
		var booking = await GetBookingAsync(bookingId);
		booking!.JobId = jobId;
		context.Bookings.Update(booking);

		await context.SaveChangesAsync();
	}

    // must be public for Hangfire
	public void UnreserveSeat(UnreserveSeatRequest request)
	{
		// log seats being unreserved
		logger.LogInformation("Unreserving seats for booking {id}.", request.BookingId);
		logger.LogInformation("Seats: {seats}", request.Seats);

		foreach (var seatCoachIdPair in request.Seats)
		{
			var coach = context.Coaches.Find(seatCoachIdPair.CoachId);
			if (coach is null)
			{
				continue;
			}

			var seat = coach.Seats.FirstOrDefault(s => s.SeatNumber == seatCoachIdPair.Seat.SeatNumber);
			if (seat is null)
			{
				continue;
			}

			seat.IsBooked = false;
			context.Coaches.Update(coach);
		}

		// mark booking as expired and delete the job
		var booking = context.Bookings.Find(request.BookingId);
		if (booking is not null)
		{
			booking.Status = BookingStatus.Expired.ToString();
			booking.JobId = null;
			context.Bookings.Update(booking);
		}
		logger.LogInformation("Unreserved seats for booking {id}", request.BookingId);
		context.SaveChanges();
	}

	public async Task<Booking?> GetBookingAsync(string bookingId)
	{
		return await context.Bookings.FindAsync(bookingId);
	}

    public async Task UpdateBookingAsync(Booking booking)
    {
        context.Bookings.Update(booking);
		await context.SaveChangesAsync();
    }

}