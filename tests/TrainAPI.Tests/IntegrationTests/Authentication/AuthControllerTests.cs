using System.Net;
using System.Net.Http.Json;

using Bogus;

using FluentAssertions;

using NUnit.Framework;

using TrainAPI.Application.ApiResponses;
using TrainAPI.Application.Features.Auth;
using TrainAPI.Application.Features.Auth.Login;
using TrainAPI.Application.Features.Auth.Register;
using TrainAPI.Domain.ServiceErrors;

namespace TrainAPI.Tests.IntegrationTests.Authentication;

[TestFixture]
public class AuthControllerTests : IntegrationTest
{
    [Test]
    public async Task Register_ValidRequestBody_ReturnsHttpOK()
    {
        // Arrange
        const string userRole = "User";

        var createUserRequest = new Faker<RegisterRequest>()
                                .CustomInstantiator(f =>
                                    new RegisterRequest
                                    {
                                        FirstName = f.Person.FirstName,
                                        LastName = f.Person.LastName,
                                        EmailAddress = f.Person.Email,
                                        Password = "testPassword12@",
                                        Role = userRole
                                    }).Generate();

        // Act
        var act = await TestClient.PostAsJsonAsync("auth/register", createUserRequest);

        // Assert
        act.EnsureSuccessStatusCode();
        act.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await act.Content.ReadFromJsonAsync<ApiResponse<UserAuthResponse>>();
        response!.Data.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Data!.AccessToken.Should().NotBeNullOrWhiteSpace();
        response.Data.Role.Should().Be(userRole);
        response.Data.Id.Should().NotBeNullOrWhiteSpace();
    }

    [Test]
    public async Task Login_ValidRequestBody_ReturnsHttpOk()
    {
        // Arrange
        await RegisterAsync();
        var loginRequest = new LoginRequest { EmailAddress = AUTH_EMAIL_ADDRESS, Password = AUTH_PASSWORD };

        // Act
        var act = await TestClient.PostAsJsonAsync("Auth/Login", loginRequest);

        // Assert
        act.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await act.Content.ReadFromJsonAsync<ApiResponse<UserAuthResponse>>();
        response!.Data.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Data!.AccessToken.Should().NotBeNullOrWhiteSpace();
        response.Data.Id.Should().NotBeNullOrWhiteSpace();
    }

    [Test]
    public async Task Login_InValidPassword_ReturnsHttpUnauthorized()
    {
        // Arrange
        await RegisterAsync();
        var loginRequest = new LoginRequest { EmailAddress = AUTH_EMAIL_ADDRESS, Password = "WrongPassword" };

        // Act
        var act = await TestClient.PostAsJsonAsync("Auth/Login", loginRequest);

        // Assert
        act.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var response = await act.Content.ReadFromJsonAsync<ApiErrorResponse>();
        response!.Errors.Should().NotBeNull();
        response.Success.Should().BeFalse();
    }

    [Test]
    public async Task Login_ThreeFailedLogins_ReturnsHttpUnauthorizedAndLockedOut()
    {
        // Arrange
        await RegisterAsync();
        var loginRequest = new LoginRequest { EmailAddress = AUTH_EMAIL_ADDRESS, Password = "WrongPassword" };

        // Act
        await TestClient.PostAsJsonAsync("Auth/Login", loginRequest);
        await TestClient.PostAsJsonAsync("Auth/Login", loginRequest);
        await TestClient.PostAsJsonAsync("Auth/Login", loginRequest);
        var act = await TestClient.PostAsJsonAsync("Auth/Login", loginRequest);

        // Assert
        act.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var response = await act.Content.ReadFromJsonAsync<ApiErrorResponse>();
        response!.Errors[0].Code.Should().Be(Errors.User.IsLockedOut.Code);
        response.Errors.Should().NotBeNull();
        response.Success.Should().BeFalse();
    }

    [Test]
    public async Task Login_UserNotRegistered_ReturnsHttpUnauthorized()
    {
        var loginRequest = new LoginRequest { EmailAddress = AUTH_EMAIL_ADDRESS, Password = AUTH_PASSWORD };

        // Act
        var act = await TestClient.PostAsJsonAsync("Auth/Login", loginRequest);

        // Assert
        act.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var response = await act.Content.ReadFromJsonAsync<ApiErrorResponse>();
        response!.Errors[0].Code.Should().Be(Errors.Auth.LoginFailed.Code);
        response.Errors.Should().NotBeNull();
        response.Success.Should().BeFalse();
    }
}