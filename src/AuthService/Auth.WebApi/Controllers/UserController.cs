using Auth.Service.Commands;
using Auth.WebApi.Dtos;
using MediatR;
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
}
