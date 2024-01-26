namespace TrainAPI.Domain.Entities;

public class Passenger
{
    public string FullName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string CoachId { get; set; } = string.Empty;
    public int SeatNumber { get; set; }
}