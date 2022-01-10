//using Auth.Service.Commands;
//using Auth.WebApi.Dtos;
//using MediatR;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace Auth.WebApi.Controllers
//{
//    [Route("[controller]")]
//    [ApiController]
//    [Authorize]
//    public class LoginController : ControllerBase
//    {
//        private readonly IMediator _mediator;

//        public LoginController(IMediator mediator)
//        {
//            _mediator = mediator;
//        }

//        [HttpPost]
//        [Route("")]
//        [AllowAnonymous]
//        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
//        {
//            if (ModelState.IsValid)
//            {
//                var command = new LoginCommand(loginDto.Email, loginDto.Password);
//                var result = await _mediator.Send(command);

//                if (result.Value == null)
//                    return new BadRequestObjectResult(result.ErrorMessages);

//                Response.Cookies.Append("X-Access-Token", result.Value.Token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
//                Response.Cookies.Append("X-User-Email", result.Value.Email, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
//                Response.Cookies.Append("X-Refresh-Token", result.Value.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
//                return new OkObjectResult(result.Value);

//            }
//            else
//                return new BadRequestObjectResult(ModelState);
//        }

//        [HttpPost]
//        [Route("Logout")]
//        public async Task<IActionResult> Logout()
//        {

//        }

//    }
//}
