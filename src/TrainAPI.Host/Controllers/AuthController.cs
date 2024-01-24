using System.Net.Mime;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TrainAPI.Application.ApiResponses;
using TrainAPI.Application.Extensions;
using TrainAPI.Application.Features.Auth;
using TrainAPI.Application.Features.Auth.Login;
using TrainAPI.Application.Features.Auth.Register;
using TrainAPI.Infrastructure;

namespace TrainAPI.Host.Controllers;

public class AuthController(IMediator mediator) : BaseController
{
    [HttpPost("register")]
    [AllowAnonymous]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<UserAuthResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
    {
        var result = await mediator.Send(request);
        return result.Match(_ => Ok(result.ToSuccessfulApiResponse()), ReturnErrorResponse);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse<UserAuthResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        var result = await mediator.Send(request);
        return result.Match(_ => Ok(result.ToSuccessfulApiResponse()), ReturnErrorResponse);
    }
}