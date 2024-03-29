namespace TrainAPI.Application.Contracts;

public interface ICurrentUser
{
    string? UserId { get; }
    string? Email { get; }
    string? Role { get; }
}