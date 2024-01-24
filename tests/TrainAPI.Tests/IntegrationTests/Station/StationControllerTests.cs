using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using NUnit.Framework;

using TrainAPI.Application.ApiResponses;
using TrainAPI.Application.Features.Stations.CreateStation;
using TrainAPI.Application.Features.Stations.GetStation;
using TrainAPI.Application.Features.Stations.UpdateStation;
using TrainAPI.Domain.Constants;

namespace TrainAPI.Tests.IntegrationTests.Station;

[TestFixture]
public class StationControllerTests : IntegrationTest
{
    private static readonly CreateStationRequest _createStationRequest =
        new() { Name = "Henry Ihenacho Station", Code = "HIS" };

    [Test]
    public async Task CreateStation_ValidRequest_ReturnsHttpCreated()
    {
        // Arrange
        await AuthenticateAsync(Roles.ADMIN);

        // Act
        var act = await TestClient.PostAsJsonAsync("Stations", _createStationRequest);
        var response = await act.Content.ReadFromJsonAsync<ApiResponse<CreateStationResponse>>();

        var getAct = await TestClient.GetAsync($"Stations/{response!.Data!.StationId}");
        var getRes = await getAct.Content.ReadFromJsonAsync<ApiResponse<GetStationResponse>>();

        // Assert
        act.EnsureSuccessStatusCode();
        getAct.EnsureSuccessStatusCode();
        act.StatusCode.Should().Be(HttpStatusCode.Created);

        response.Data.Should().NotBeNull();
        response.Data!.StationId.Should().NotBeNull();
        response.Success.Should().BeTrue();

        getRes!.Data!.Id.Should().Be(response.Data.StationId);
        getRes.Success.Should().BeTrue();
    }

    [Test]
    public async Task UpdateStation_StationDoesNotExist_ReturnsHttpNotFound()
    {
        // Arrange
        await AuthenticateAsync(Roles.ADMIN);

        // Act
        var act = await TestClient.PutAsJsonAsync($"Stations/id", new UpdateStationRequest { StationId = "" });
        var response = await act.Content.ReadFromJsonAsync<ApiErrorResponse>();

        // Assert
        act.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response!.Success.Should().BeFalse();
    }

    [Test]
    public async Task UpdateStation_StationExistsAndValidDetails_ReturnsHttpNoContent()
    {
        // Arrange
        await AuthenticateAsync(Roles.ADMIN);
        const string newName = "New Name";

        // Act
        var createAct = await TestClient.PostAsJsonAsync("Stations", _createStationRequest);
        var createResponse = await createAct.Content.ReadFromJsonAsync<ApiResponse<CreateStationResponse>>();

        var updateAct =
            await TestClient.PutAsJsonAsync($"Stations/{createResponse!.Data!.StationId}",
                new UpdateStationRequestDto { Name = newName });

        // Assert
        createAct.EnsureSuccessStatusCode();
        createAct.StatusCode.Should().Be(HttpStatusCode.Created);

        updateAct.EnsureSuccessStatusCode();
        updateAct.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getAct = await TestClient.GetAsync($"Stations/{createResponse.Data!.StationId}");
        var getResponse = await getAct.Content.ReadFromJsonAsync<ApiResponse<GetStationResponse>>();

        getResponse!.Data.Should().NotBeNull();
        getResponse.Data!.Name.Should().Be(newName);
        // original values should be unchanged
        getResponse.Data.Code.Should().Be(_createStationRequest.Code);
    }

    [Test]
    public async Task GetStation_StationDoesNotExist_ReturnsHttpNotFound()
    {
        // Arrange
        await AuthenticateAsync(Roles.ADMIN);

        // Act
        var getAct = await TestClient.GetAsync($"Stations/notARealId");
        var getRes = await getAct.Content.ReadFromJsonAsync<ApiResponse<GetStationResponse>>();

        // Assert
        getAct.StatusCode.Should().Be(HttpStatusCode.NotFound);
        getRes!.Success.Should().BeFalse();
    }

    [Test]
    public async Task GetStation_StationExists_ReturnsHttpOk()
    {
        // Arrange
        await AuthenticateAsync(Roles.ADMIN);

        // Act
        var createStationResponse = await CreateStation(_createStationRequest);
        var getAct = await TestClient.GetAsync($"Stations/{createStationResponse.StationId}");
        var getRes = await getAct.Content.ReadFromJsonAsync<ApiResponse<GetStationResponse>>();

        // Assert
        getAct.EnsureSuccessStatusCode();
        getRes!.Success.Should().BeTrue();
        getRes.Data!.Id.Should().Be(createStationResponse.StationId);
    }

    [Test]
    public async Task DeleteStation_StationExists_ReturnsNoContent()
    {
        // Arrange
        await AuthenticateAsync(Roles.ADMIN);

        // Act
        var createStationResponse = await CreateStation(_createStationRequest);
        var deleteAct = await TestClient.DeleteAsync($"Stations/{createStationResponse.StationId}");

        // Assert
        deleteAct.EnsureSuccessStatusCode();
        deleteAct.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Test]
    public async Task DeleteStation_StationDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        await AuthenticateAsync(Roles.ADMIN);

        // Act
        var deleteAct = await TestClient.DeleteAsync($"Stations/MissingId");

        // Assert
        deleteAct.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task GetAllStations_StationsExist_ReturnsListOfStations()
    {
        // Arrange
        await AuthenticateAsync(Roles.ADMIN);

        // Act
        await CreateStation(_createStationRequest);
        await CreateStation(_createStationRequest);

        var getAct = await TestClient.GetAsync("Stations/all");
        var getRes = await getAct.Content.ReadFromJsonAsync<ApiResponse<PagedResponse<GetStationResponse>>>();

        // Assert
        getAct.EnsureSuccessStatusCode();
        getAct.StatusCode.Should().Be(HttpStatusCode.OK);
        getRes!.Success.Should().BeTrue();
        getRes.Data!.TotalCount.Should().Be(2);
    }
}