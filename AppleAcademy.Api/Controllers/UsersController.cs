using AppleAcademy.Api.Middleware;
using AppleAcademy.Application.Dtos;
using AppleAcademy.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppleAcademy.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : BaseController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var users = await _userService.GetAllAsync();

        return Ok(Result<ICollection<UserDto>>.Ok(users));
    }

    [HttpGet("{guid}")]
    public async Task<IActionResult> GetByGuid(Guid guid)
    {
        var user = await _userService.GetByGuidAsync(guid);

        return user is null
            ? NotFound(Result<object>.Failure("Usuário não encontrado."))
            : Ok(Result<UserDto>.Ok(user));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UserCreateDto userCreateDto)
    {
        var user = await _userService.AddAsync(userCreateDto);

        return CreatedAtAction(nameof(GetByGuid), new { guid = user.Guid }, Result<UserDto>.Ok(user));
    }

    [AllowAnonymous]
    [HttpPost("[Action]")]
    public async Task<IActionResult> SignUp([FromBody] UserCreateDto userCreateDto)
    {
        return await Post(userCreateDto);
    }

    [HttpPut("{guid}")]
    public async Task<IActionResult> Put(Guid guid, [FromBody] UserUpdateDto userUpdateDto)
    {
        var user = await _userService.UpdateAsync(guid, userUpdateDto);

        return user is null
            ? NotFound(Result<object>.Failure("Usuário não encontrado."))
            : Ok(Result<UserDto>.Ok(user));
    }    

    [HttpDelete("{guid}")]
    public async Task<IActionResult> Delete(Guid guid)
    {
        var deleted = await _userService.DeleteAsync(guid);

        return deleted
            ? NoContent()
            : NotFound(Result<object>.Failure("Usuário não encontrado."));
    }
}
