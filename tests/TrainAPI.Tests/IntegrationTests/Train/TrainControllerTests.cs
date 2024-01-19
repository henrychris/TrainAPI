using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using NUnit.Framework;

using TrainAPI.Application.ApiResponses;
using TrainAPI.Application.Features.Trains.CreateTrain;
using TrainAPI.Application.Features.Trains.GetSingleTrain;
using TrainAPI.Application.Features.Trains.UpdateTrain;
using TrainAPI.Domain.Constants;

namespace TrainAPI.Tests.IntegrationTests.Train;

[TestFixture]
public class TrainControllerTests : IntegrationTest
{
    private static readonly CreateTrainRequest _createTrainRequest =
        new() { Name = "Lagos-Ibadan", Code = "LI1" };

    [Test]
    public async Task CreateTrain_ValidRequest_ReturnsHttpCreated()
    {
        // Arrange
        await AuthenticateAsync(Roles.ADMIN);

        // Act
        var act = await TestClient.PostAsJsonAsync("Trains", _createTrainRequest);
        var response = await act.Content.ReadFromJsonAsync<ApiResponse<CreateTrainResponse>>();

        var getAct = await TestClient.GetAsync($"Trains/{response!.Data!.TrainId}");
        var getRes = await getAct.Content.ReadFromJsonAsync<ApiResponse<GetTrainResponse>>();

        // Assert
        act.EnsureSuccessStatusCode();
        getAct.EnsureSuccessStatusCode();
        act.StatusCode.Should().Be(HttpStatusCode.Created);

        response.Data.Should().NotBeNull();
        response.Data!.TrainId.Should().NotBeNull();
        response.Success.Should().BeTrue();

        getRes!.Data!.Id.Should().Be(response.Data.TrainId);
        getRes.Success.Should().BeTrue();
    }

    [Test]
    public async Task UpdateTrain_TrainDoesNotExist_ReturnsHttpNotFound()
    {
        // Arrange
        await AuthenticateAsync(Roles.ADMIN);

        // Act
        var act = await TestClient.PutAsJsonAsync($"Trains/id", new UpdateTrainRequest());
        var response = await act.Content.ReadFromJsonAsync<ApiErrorResponse>();

        // Assert
        act.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response!.Success.Should().BeFalse();
    }

    [Test]
    public async Task UpdateTrain_TrainExistsAndValidDetails_ReturnsHttpNoContent()
    {
        // Arrange
        await AuthenticateAsync(Roles.ADMIN);
        const string newName = "New Name";

        // Act
        var createResponse = await CreateTrain(_createTrainRequest);
        var updateAct =
            await TestClient.PutAsJsonAsync($"Trains/{createResponse.TrainId}",
                new UpdateTrainRequestDto { Name = newName });
        var getAct = await TestClient.GetAsync($"Trains/{createResponse.TrainId}");
        var getResponse = await getAct.Content.ReadFromJsonAsync<ApiResponse<GetTrainResponse>>();

        // Assert
        updateAct.EnsureSuccessStatusCode();
        updateAct.StatusCode.Should().Be(HttpStatusCode.NoContent);
        getResponse!.Data.Should().NotBeNull();
        getResponse.Data!.Name.Should().Be(newName);
        // original values should be unchanged
        getResponse.Data.Code.Should().Be(_createTrainRequest.Code);
    }

    [Test]
    public async Task GetTrain_TrainDoesNotExist_ReturnsHttpNotFound()
    {
        // Arrange
        await AuthenticateAsync(Roles.ADMIN);

        // Act
        var getAct = await TestClient.GetAsync($"Trains/notARealId");
        var getRes = await getAct.Content.ReadFromJsonAsync<ApiResponse<GetTrainResponse>>();

        // Assert
        getAct.StatusCode.Should().Be(HttpStatusCode.NotFound);
        getRes!.Success.Should().BeFalse();
    }

    [Test]
    public async Task GetTrain_TrainExists_ReturnsHttpOk()
    {
        // Arrange
        await AuthenticateAsync(Roles.ADMIN);

        // Act
        var createTrainResponse = await CreateTrain(_createTrainRequest);
        var getAct = await TestClient.GetAsync($"Trains/{createTrainResponse.TrainId}");
        var getRes = await getAct.Content.ReadFromJsonAsync<ApiResponse<GetTrainResponse>>();

        // Assert
        getAct.EnsureSuccessStatusCode();
        getRes!.Success.Should().BeTrue();
        getRes.Data!.Id.Should().Be(createTrainResponse.TrainId);
    }

    [Test]
    public async Task DeleteTrain_TrainExists_ReturnsNoContent()
    {
        // Arrange
        await AuthenticateAsync(Roles.ADMIN);

        // Act
        var createTrainResponse = await CreateTrain(_createTrainRequest);
        var deleteAct = await TestClient.DeleteAsync($"Trains/{createTrainResponse.TrainId}");

        // Assert
        deleteAct.EnsureSuccessStatusCode();
        deleteAct.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Test]
    public async Task DeleteTrain_TrainDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        await AuthenticateAsync(Roles.ADMIN);

        // Act
        var deleteAct = await TestClient.DeleteAsync($"Trains/MissingId");

        // Assert
        deleteAct.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task GetAllTrains_TrainsExist_ReturnsListOfTrains()
    {
        // Arrange
        await AuthenticateAsync(Roles.ADMIN);

        // Act
        await CreateTrain(_createTrainRequest);
        await CreateTrain(_createTrainRequest);

        var getAct = await TestClient.GetAsync("Trains/all");
        var getRes = await getAct.Content.ReadFromJsonAsync<ApiResponse<PagedResponse<GetTrainResponse>>>();

        // Assert
        getAct.EnsureSuccessStatusCode();
        getAct.StatusCode.Should().Be(HttpStatusCode.OK);
        getRes!.Success.Should().BeTrue();
        getRes.Data!.TotalCount.Should().Be(2);
    }
}