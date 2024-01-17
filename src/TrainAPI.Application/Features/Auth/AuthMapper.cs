using TrainAPI.Application.Extensions;
using TrainAPI.Application.Features.Auth.Register;
using TrainAPI.Domain.Entities;

namespace TrainAPI.Application.Features.Auth;

public static class AuthMapper
{
    public static ApplicationUser MapToApplicationUser(RegisterRequest request)
    {
        return new ApplicationUser
        {
            FirstName = request.FirstName.FirstCharToUpper(),
            LastName = request.LastName.FirstCharToUpper(),
            Email = request.EmailAddress,
            UserName = request.EmailAddress,
            Role = request.Role
        };
    }
}