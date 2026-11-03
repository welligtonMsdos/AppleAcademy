using AppleAcademy.Api.Middleware;
using AppleAcademy.Application.Dtos;
using AppleAcademy.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppleAcademy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class PerfisController : Controller
{
    private readonly IPerfilService _perfilService;

    public PerfisController(IPerfilService perfilService)
    {
        _perfilService = perfilService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var perfis = await _perfilService.GetAllAsync();

        return Ok(Result<ICollection<PerfilDto>>.Ok(perfis));
    }

    [HttpGet("{guid}")]
    public async Task<IActionResult> GetByGuid(Guid guid)
    {
        var perfil = await _perfilService.GetByGuidAsync(guid);

        return perfil is null
            ? NotFound(Result<object>.Failure("Perfil não encontrado."))
            : Ok(Result<PerfilDto>.Ok(perfil));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PerfilCreateDto perfilCreateDto)
    {
        var perfil = await _perfilService.AddAsync(perfilCreateDto);

        return CreatedAtAction(nameof(GetByGuid), new { guid = perfil.Guid }, Result<PerfilDto>.Ok(perfil));
    }

    [HttpPut("{guid}")]
    public async Task<IActionResult> Put(Guid guid, [FromBody] PerfilUpdateDto perfilUpdateDto)
    {
        var perfil = await _perfilService.UpdateAsync(guid, perfilUpdateDto);

        return perfil is null
            ? NotFound(Result<object>.Failure("Perfil não encontrado."))
            : Ok(Result<PerfilDto>.Ok(perfil));
    }

    [HttpDelete("{guid}")]
    public async Task<IActionResult> Delete(Guid guid)
    {
        var deleted = await _perfilService.DeleteAsync(guid);

        return deleted
            ? NoContent()
            : NotFound(Result<object>.Failure("Perfil não encontrado."));
    }
}
