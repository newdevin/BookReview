using Auth.Service.Commands;
using Auth.WebApi.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Auth.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class RegisterController : ControllerBase
{
    private readonly IMediator _mediatr;

    public RegisterController(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
            return new BadRequestObjectResult(ModelState);

        RegisterCommand command = new(registerDto.Email, registerDto.FirstName, registerDto.LastName, registerDto.Password, registerDto.RepeatPassword);
        var result = await _mediatr.Send(command);

        return (result.Value != null) ? new OkObjectResult(result.Value) : new BadRequestObjectResult(result.ErrorMessages);
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("verify/{base64Email}/{code}")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto verifyEmailDto)
    {
        if (!ModelState.IsValid)
            return new BadRequestObjectResult(ModelState);

        var command = new VerifyEmailCommand(verifyEmailDto.Email, verifyEmailDto.Code);
        var result = await _mediatr.Send(command);

        if (result.Value == false)
            return new BadRequestObjectResult(result.ErrorMessages);

        return new OkResult();
    }
}
