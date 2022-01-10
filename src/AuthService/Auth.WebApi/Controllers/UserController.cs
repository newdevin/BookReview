using Auth.Service.Commands;
using Auth.WebApi.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
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
    public async Task<ActionResult> RefreshAccessToken([FromBody] RefreshAccessTokenDto refreshAccessTokenDto)
    {
        if (!ModelState.IsValid)
            return new BadRequestObjectResult(ModelState);

        if (!Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshToken))
            return BadRequest();

        var email = refreshAccessTokenDto.Email;

        if (string.IsNullOrEmpty(email) || refreshToken == null)
            return BadRequest();

        var command = new RefreshTokenCommand(email, refreshToken);
        var result = await _mediator.Send(command);

        if (result.Value == null)
            return new BadRequestObjectResult(result.ErrorMessages);

        AddCookie("X-Access-Token", result.Value.Token);
        AddCookie("X-Refresh-Token", result.Value.RefreshToken);
        return new OkObjectResult(result.Value);

    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (ModelState.IsValid)
        {
            var command = new LoginCommand(loginDto.Email, loginDto.Password);
            var result = await _mediator.Send(command);

            if (result.Value == null)
                return new BadRequestObjectResult(result.ErrorMessages);

            AddCookie("X-Access-Token", result.Value.Token);
            AddCookie("X-Refresh-Token", result.Value.RefreshToken);

            return new OkObjectResult(result.Value);
        }
        else
            return new BadRequestObjectResult(ModelState);
    }

    [HttpPost]
    [Route("Logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutDto logoutDto)
    {
        //remove refresh token
        //delete cookies.
        if(!ModelState.IsValid)
            return new BadRequestObjectResult(ModelState);

        var command = new LogoutCommand(logoutDto.Email);
        var result = await _mediator.Send(command);
        
        if (result.Value == false)
            return new BadRequestObjectResult(result.ErrorMessages);

        ExpireCookie("X-Access-Token");
        ExpireCookie("X-Refresh-Token");

        return Ok();
    }

    private void AddCookie(string name, string value)
    {
        Response.Cookies.Append(name, value, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
    }

    private void ExpireCookie(string name)
    {
        Response.Cookies.Append(name, "", new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict, Expires = DateTime.UtcNow.AddDays(-2) });
    }
}
