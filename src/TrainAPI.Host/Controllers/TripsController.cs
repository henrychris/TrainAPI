using System.Net.Mime;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TrainAPI.Application.ApiResponses;
using TrainAPI.Application.Extensions;
using TrainAPI.Application.Features.Trips;
using TrainAPI.Application.Features.Trips.CreateTrip;
using TrainAPI.Application.Features.Trips.DeleteTrip;
using TrainAPI.Application.Features.Trips.GetAllTrips;
using TrainAPI.Application.Features.Trips.GetSingleTrip;
using TrainAPI.Application.Features.Trips.UpdateTrip;
using TrainAPI.Domain.Constants;
using TrainAPI.Infrastructure;

namespace TrainAPI.Host.Controllers;

public class TripsController(IMediator mediator) : BaseController
{
    [Authorize(Roles = Roles.ADMIN)]
    [HttpPost]
    [AllowAnonymous]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<CreateTripResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateTrip([FromBody] CreateTripRequest request)
    {
        var result = await mediator.Send(request);

        return result.Match(
            response => CreatedAtAction(nameof(GetSingleTrip),
                routeValues: new { id = response.TripId },
                result.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<GetTripResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSingleTrip(string id)
    {
        var result = await mediator.Send(new GetSingleTripRequest { TripId = id });

        // If successful, return the event data in an ApiResponse.
        // If an error occurs, return an error response using the ReturnErrorResponse method.
        return result.Match(
            _ => Ok(result.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }

    [Authorize(Roles = Roles.ADMIN)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteTrip(string id)
    {
        var result = await mediator.Send(new DeleteTripRequest { TripId = id });
        return result.Match(_ => NoContent(), ReturnErrorResponse);
    }

    [Authorize(Roles = Roles.ADMIN)]
    [HttpGet("all")]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<GetTripResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTrips([FromQuery] GetAllTripsRequest request)
    {
        var result = await mediator.Send(request);

        // If successful, return the event data in an ApiResponse.
        // If an error occurs, return an error response using the ReturnErrorResponse method.
        return result.Match(
            _ => Ok(result.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }

    [Authorize(Roles = Roles.ADMIN)]
    [HttpPut("{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateTrip(string id, [FromBody] UpdateTripRequestDto request)
    {
        var result = await mediator.Send(TripMapper.ToUpdateTripRequest(request, id));
        return result.Match(_ => NoContent(), ReturnErrorResponse);
    }
}