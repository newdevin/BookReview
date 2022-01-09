using Auth.Service.Commands;
using Auth.WebApi.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("updatepassword")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        if (ModelState.IsValid)
        {
            var command = new ChangePasswordCommand(dto.Email, dto.OriginalPassword, dto.Password, dto.RepeatPassword);
            var result = await _mediator.Send(command);
            return result.Value == null ? new BadRequestObjectResult(result.ErrorMessages) : new OkResult();
        }
        else
            return new BadRequestObjectResult(ModelState);
    }

    [HttpPost]
    [Route("refreshtoken")]
    public async Task<ActionResult> RefreshToken()
    {
        if (!(Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshToken) && Request.Cookies.TryGetValue("X-User-Email", out var email)))
            return BadRequest();

        if(string.IsNullOrEmpty(email) || refreshToken == null)
            return BadRequest();

        var command = new RefreshTokenCommand(email, refreshToken);
        var result = await _mediator.Send(command);

        if (result.Value == null)
            return new BadRequestObjectResult(result.ErrorMessages);

        Response.Cookies.Append("X-Access-Token", result.Value.Token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
        Response.Cookies.Append("X-User-Email", result.Value.Email, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
        Response.Cookies.Append("X-Refresh-Token", result.Value.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
        return new OkObjectResult(result.Value);

    }
}
