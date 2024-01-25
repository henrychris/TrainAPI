using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using NUnit.Framework;

using TrainAPI.Application.ApiResponses;
using TrainAPI.Application.Features.Coaches.CreateCoach;
using TrainAPI.Application.Features.Trains.CreateTrain;
using TrainAPI.Domain.Constants;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.Enums;

namespace TrainAPI.Tests.IntegrationTests.Coaches;

[TestFixture]
public class CoachControllerTests : IntegrationTest
{
    private static readonly CreateTrainRequest _createTrainRequest =
        new() { Name = "Lagos-Ibadan", Code = "LI1" };

    [Test]
    public async Task CreateCoach_ValidRequest_ReturnsHttpCreated()
    {
        // Arrange
        await AuthenticateAsync(Roles.ADMIN);

        // Act
        var train = await CreateTrain(_createTrainRequest);

        var act = await TestClient.PostAsJsonAsync("Coaches", new CreateCoachRequest
        {
            Name = "Henry Ihenacho Coach",
            Class = CoachClass.First.ToString(),
            SeatCount = 10,
            TravellerCategories =
            [
                new TravellerPairs { Type = TravellerCategory.Adult.ToString(), Price = 20 },
                new TravellerPairs { Type = TravellerCategory.Child.ToString(), Price = 20 }
            ],
            TrainId = train.TrainId
        });

        var response = await act.Content.ReadFromJsonAsync<ApiResponse<CreateCoachResponse>>();
        var coach = await GetCoach(response.Data.CoachId);

        // Assert
        act.EnsureSuccessStatusCode();
        act.StatusCode.Should().Be(HttpStatusCode.Created);

        response!.Data.Should().NotBeNull();
        response.Data!.CoachId.Should().NotBeNull();
        response.Success.Should().BeTrue();

        coach.AvailableSeats.Should().Be(10);
        coach.Seats.Count.Should().Be(10);
    }

    [Test]
    public async Task CreateCoach_CoachClassIsInvalid_ReturnsHttpBadRequest()
    {
        // Arrange
        await AuthenticateAsync(Roles.ADMIN);

        // Act
        var train = await CreateTrain(_createTrainRequest);

        var act = await TestClient.PostAsJsonAsync("Coaches", new CreateCoachRequest
        {
            Name = "Henry Ihenacho Coach",
            Class = "Invalid class",
            SeatCount = 10,
            TravellerCategories =
            [
                new TravellerPairs { Type = TravellerCategory.Adult.ToString(), Price = 20 },
                new TravellerPairs { Type = TravellerCategory.Child.ToString(), Price = 20 }
            ],
            TrainId = train.TrainId
        });

        var response = await act.Content.ReadFromJsonAsync<ApiErrorResponse>();

        // Assert
        act.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response!.Success.Should().BeFalse();
    }

    [Test]
    public async Task CreateCoach_TrainDoesNotExist_ReturnsHttpNotFound()
    {
        // Arrange
        await AuthenticateAsync(Roles.ADMIN);

        // Act
        var act = await TestClient.PostAsJsonAsync("Coaches", new CreateCoachRequest
        {
            Name = "Henry Ihenacho Coach",
            Class = CoachClass.First.ToString(),
            SeatCount = 10,
            TravellerCategories =
            [
                new TravellerPairs { Type = TravellerCategory.Adult.ToString(), Price = 20 },
                new TravellerPairs { Type = TravellerCategory.Child.ToString(), Price = 20 }
            ],
            TrainId = "id"
        });

        var response = await act.Content.ReadFromJsonAsync<ApiErrorResponse>();

        // Assert
        act.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response!.Success.Should().BeFalse();
    }

    // todo: remember to test validators separately.
}