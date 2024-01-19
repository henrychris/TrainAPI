using System.Net.Mime;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TrainAPI.Application.ApiResponses;
using TrainAPI.Application.Extensions;
using TrainAPI.Application.Features.Trains.CreateTrain;
using TrainAPI.Application.Features.Trains.DeleteTrain;
using TrainAPI.Application.Features.Trains.GetAllTrains;
using TrainAPI.Application.Features.Trains.GetSingleTrain;
using TrainAPI.Application.Features.Trains.UpdateTrain;
using TrainAPI.Domain.Constants;
using TrainAPI.Infrastructure;

namespace TrainAPI.Host.Controllers;

public class TrainsController(IMediator mediator) : BaseController
{
    [Authorize(Roles = Roles.ADMIN)]
    [HttpPost]
    [AllowAnonymous]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<CreateTrainResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateTrain([FromBody] CreateTrainRequest request)
    {
        var result = await mediator.Send(request);

        return result.Match(
            response => CreatedAtAction(nameof(GetSingleTrain),
                routeValues: new { id = response.TrainId },
                result.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<GetTrainResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSingleTrain(string id)
    {
        var result = await mediator.Send(new GetSingleTrainRequest { TrainId = id });

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
    public async Task<IActionResult> UpdateTrain(string id, [FromBody] UpdateTrainRequestDto request)
    {
        var result = await mediator.Send(new UpdateTrainRequest
        {
            TrainId = id, Code = request.Code, Name = request.Name
        });
        return result.Match(_ => NoContent(), ReturnErrorResponse);
    }

    [Authorize(Roles = Roles.ADMIN)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteTrain(string id)
    {
        var result = await mediator.Send(new DeleteTrainRequest { TrainId = id });
        return result.Match(_ => NoContent(), ReturnErrorResponse);
    }

    [Authorize(Roles = Roles.ADMIN)]
    [HttpGet("all")]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<GetTrainResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTrains([FromQuery] GetAllTrainsRequest request)
    {
        var result = await mediator.Send(request);

        // If successful, return the event data in an ApiResponse.
        // If an error occurs, return an error response using the ReturnErrorResponse method.
        return result.Match(
            _ => Ok(result.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }
}