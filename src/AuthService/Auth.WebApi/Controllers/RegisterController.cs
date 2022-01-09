using Auth.Service.Commands;
using Auth.WebApi.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (ModelState.IsValid)
        {
            RegisterCommand command = new(registerDto.Email, registerDto.Password, registerDto.RepeatPassword);
            var result = await _mediatr.Send(command);
            if (result.Value != null)
                return new CreatedResult("login", default);
            else
                return new BadRequestObjectResult(result.ErrorMessages);
        }
        else
            return new BadRequestObjectResult(ModelState);
    }
}
