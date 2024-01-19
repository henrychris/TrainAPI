namespace TrainAPI.Application.Interfaces;

public interface ICurrentUser
{
    string? UserId { get; }
    string? Email { get; }
    string? Role { get; }
}