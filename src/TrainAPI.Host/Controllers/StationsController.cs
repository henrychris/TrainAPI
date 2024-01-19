using System.Net.Mime;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TrainAPI.Application.ApiResponses;
using TrainAPI.Application.Extensions;
using TrainAPI.Application.Features.Stations.CreateStation;
using TrainAPI.Application.Features.Stations.DeleteStation;
using TrainAPI.Application.Features.Stations.GetAllStations;
using TrainAPI.Application.Features.Stations.GetStation;
using TrainAPI.Application.Features.Stations.UpdateStation;
using TrainAPI.Domain.Constants;
using TrainAPI.Infrastructure;

namespace TrainAPI.Host.Controllers;

public class StationsController(IMediator mediator) : BaseController
{
    [Authorize(Roles = Roles.ADMIN)]
    [HttpPost]
    [AllowAnonymous]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<CreateStationResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateStation([FromBody] CreateStationRequest request)
    {
        var result = await mediator.Send(request);

        return result.Match(
            response => CreatedAtAction(nameof(GetSingleStation),
                routeValues: new { id = response.StationId },
                result.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<GetStationResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSingleStation(string id)
    {
        var result = await mediator.Send(new GetSingleStationRequest { StationId = id });

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
    public async Task<IActionResult> UpdateMovie(string id, [FromBody] UpdateStationRequestDto request)
    {
        var result = await mediator.Send(new UpdateStationRequest
        {
            StationId = id, Code = request.Code, Name = request.Code
        });
        return result.Match(_ => NoContent(), ReturnErrorResponse);
    }

    [Authorize(Roles = Roles.ADMIN)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteStation(string id)
    {
        var result = await mediator.Send(new DeleteStationRequest { StationId = id });
        return result.Match(_ => NoContent(), ReturnErrorResponse);
    }

    [Authorize(Roles = Roles.ADMIN)]
    [HttpGet("all")]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<GetStationResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllStations([FromQuery] GetAllStationsRequest request)
    {
        var result = await mediator.Send(request);

        // If successful, return the event data in an ApiResponse.
        // If an error occurs, return an error response using the ReturnErrorResponse method.
        return result.Match(
            _ => Ok(result.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }
}