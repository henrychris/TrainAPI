using System.Net.Mime;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TrainAPI.Application.ApiResponses;
using TrainAPI.Application.Extensions;
using TrainAPI.Application.Features.Bookings.GetSingleBooking;
using TrainAPI.Application.Features.Bookings.InitialiseBooking;
using TrainAPI.Domain.Constants;
using TrainAPI.Infrastructure;

namespace TrainAPI.Host.Controllers;

public class BookingController(IMediator mediator) : BaseController
{
    // send a request with: tripId, passengers & seats, validate
    // return "ticket"
    [Authorize]
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<InitialiseBookingResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> InitialiseBooking([FromBody] InitialiseBookingRequest request)
    {
        var result = await mediator.Send(request);

        return result.Match(
            response => CreatedAtAction(nameof(GetSingleBooking),
                routeValues: new { id = response.BookingId },
                result.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }

    [Authorize(Roles = Roles.ADMIN)]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<GetBookingResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSingleBooking(string id)
    {
        var result = await mediator.Send(new GetSingleBookingRequest { BookingId = id });

        // If successful, return the event data in an ApiResponse.
        // If an error occurs, return an error response using the ReturnErrorResponse method.
        return result.Match(
            _ => Ok(result.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }
}