using AppleAcademy.Api.Middleware;
using AppleAcademy.Application.Dtos;
using AppleAcademy.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AppleAcademy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseController
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("[Action]")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        var token = await _userService.LoginAsync(userLoginDto);

        return Ok(Result<string>.Ok(token));
    }
}
