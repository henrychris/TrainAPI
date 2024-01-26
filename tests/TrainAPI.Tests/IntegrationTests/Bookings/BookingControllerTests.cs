using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using NUnit.Framework;

using TrainAPI.Application.ApiResponses;
using TrainAPI.Application.Features.Bookings.InitialiseBooking;
using TrainAPI.Application.Features.Coaches.CreateCoach;
using TrainAPI.Application.Features.Stations.CreateStation;
using TrainAPI.Application.Features.Trains.CreateTrain;
using TrainAPI.Application.Features.Trips.CreateTrip;
using TrainAPI.Domain.Constants;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.Enums;

namespace TrainAPI.Tests.IntegrationTests.Bookings
{
    [TestFixture]
    public class BookingControllerTests : IntegrationTest
    {
        private static readonly CreateStationRequest _fromStationRequest =
            new() { Name = "Henry Ihenacho Station", Code = "HIS" };

        private static readonly CreateStationRequest _toStationRequest =
            new() { Name = "Other Ihenacho Station", Code = "OIS" };

        private static readonly CreateTrainRequest _trainRequest =
            new() { Name = "Henry Ihenacho Train", Code = "HIT" };

        [Test]
        public async Task CreateBooking_ValidRequest_ReturnsHttpCreated()
        {
            // Arrange
            await AuthenticateAsync(Roles.ADMIN);

            // Act
            var toStation = await CreateStation(_toStationRequest);
            var fromStation = await CreateStation(_fromStationRequest);
            var train = await CreateTrain(_trainRequest);
            var coachReq = new CreateCoachRequest
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
            };
            var coach = await CreateCoach(coachReq);

            var tripReq = new CreateTripRequest
            {
                FromStationId = fromStation.StationId,
                ToStationId = toStation.StationId,
                TrainId = train.TrainId,
                DepartureTime = DateTime.UtcNow.AddHours(1),
                ArrivalTime = DateTime.UtcNow.AddHours(2),
                Name = "Lagos-Ibadan",
                DistanceInKilometers = 400,
                DateOfTrip = DateTime.UtcNow
            };
            var trip = await CreateTrip(tripReq);

            var act = await TestClient.PostAsJsonAsync("Booking", new InitialiseBookingRequest
            {
                Name = "Henry Ihenacho",
                ContactInfo = new ContactDetails { EmailAddress = "test@email.com", PhoneNumber = "1234567890" },
                TripId = trip.TripId,
                Passengers =
                [
                    new()
                    {
                        FullName = "Chris Ihenacho",
                        SeatNumber = 1,
                        CoachId = coach.CoachId,
                        EmailAddress = "test@email.com",
                        PhoneNumber = "1234567890"
                    }
                ]
            });

            var response = await act.Content.ReadFromJsonAsync<ApiResponse<InitialiseBookingResponse>>();

            // Assert
            act.EnsureSuccessStatusCode();
            act.StatusCode.Should().Be(HttpStatusCode.Created);

            response!.Data.Should().NotBeNull();
            response.Data!.BookingId.Should().NotBeNull();
            response.Success.Should().BeTrue();
        }

        [Test]
        public async Task CreateBooking_InvalidRequest_ReturnsHttpBadRequest()
        {
            // Arrange
            await AuthenticateAsync(Roles.ADMIN);

            // Act
            var act = await TestClient.PostAsJsonAsync("Booking", new InitialiseBookingRequest
            {
                Name = "Henry Ihenacho",
                ContactInfo = new ContactDetails { EmailAddress = "testemail.com", PhoneNumber = "1234567890" },
                TripId = "123",
                Passengers =
                [
                    new()
                    {
                        FullName = "Chris Ihenacho",
                        SeatNumber = 1,
                        CoachId = "123",
                        EmailAddress = "testemail.com",
                        PhoneNumber = "1234567890"
                    }
                ]
            });

            var response = await act.Content.ReadFromJsonAsync<ApiErrorResponse>();

            // Assert
            act.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response!.Success.Should().BeFalse();
        }
    }
}