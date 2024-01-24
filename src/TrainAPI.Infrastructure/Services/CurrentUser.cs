using TrainAPI.Application.Contracts;
using TrainAPI.Domain.Constants;

namespace TrainAPI.Infrastructure.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public string? UserId =>
        httpContextAccessor.HttpContext?.User.FindFirst(JwtClaims.USER_ID)?.Value;

    public string? Email =>
        httpContextAccessor.HttpContext?.User.FindFirst(JwtClaims.EMAIL)?.Value;

    public string? Role =>
        httpContextAccessor.HttpContext?.User.FindFirst(JwtClaims.ROLE)?.Value;
}