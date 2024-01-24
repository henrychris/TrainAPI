namespace TrainAPI.Domain.Entities;

public class Seat
{
    public bool IsBooked { get; set; }
    public required int SeatNumber { get; set; }
}