using System.Net.Mime;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TrainAPI.Application.ApiResponses;
using TrainAPI.Application.Extensions;
using TrainAPI.Application.Features.Coaches;
using TrainAPI.Application.Features.Coaches.CreateCoach;
using TrainAPI.Application.Features.Coaches.DeleteCoach;
using TrainAPI.Application.Features.Coaches.GetAllCoaches;
using TrainAPI.Application.Features.Coaches.GetSingleCoach;
using TrainAPI.Application.Features.Coaches.UpdateCoach;
using TrainAPI.Domain.Constants;
using TrainAPI.Infrastructure;

namespace TrainAPI.Host.Controllers;

public class CoachesController(IMediator mediator) : BaseController
{
    [Authorize(Roles = Roles.ADMIN)]
    [HttpPost]
    [AllowAnonymous]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<CreateCoachResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateCoach([FromBody] CreateCoachRequest request)
    {
        var result = await mediator.Send(request);

        return result.Match(
            response => CreatedAtAction(nameof(GetSingleCoach),
                routeValues: new { id = response.CoachId },
                result.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<GetCoachResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSingleCoach(string id)
    {
        var result = await mediator.Send(new GetSingleCoachRequest { CoachId = id });

        // If successful, return the event data in an ApiResponse.
        // If an error occurs, return an error response using the ReturnErrorResponse method.
        return result.Match(
            _ => Ok(result.ToSuccessfulApiResponse()),
            ReturnErrorResponse);
    }

    [Authorize(Roles = Roles.ADMIN)]
    [HttpGet("all")]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<GetCoachResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCoaches([FromQuery] GetAllCoachesRequest request)
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
    public async Task<IActionResult> UpdateCoach(string id, [FromBody] UpdateCoachRequestDto request)
    {
        var result = await mediator.Send(CoachMapper.CreateUpdateCoachRequest(id, request));
        return result.Match(_ => NoContent(), ReturnErrorResponse);
    }

    [Authorize(Roles = Roles.ADMIN)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteCoach(string id)
    {
        var result = await mediator.Send(new DeleteCoachRequest { CoachId = id });
        return result.Match(_ => NoContent(), ReturnErrorResponse);
    }
}