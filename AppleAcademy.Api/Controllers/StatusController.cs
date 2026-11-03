using AppleAcademy.Api.Middleware;
using AppleAcademy.Application.Dtos;
using AppleAcademy.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppleAcademy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class StatusController : ControllerBase
{
    private readonly IStatusService _statusService;

    public StatusController(IStatusService statusService)
    {
        _statusService = statusService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var statuses = await _statusService.GetAllAsync();
        return Ok(Result<ICollection<StatusDto>>.Ok(statuses));
    }

    [HttpGet("{guid:guid}")]
    public async Task<IActionResult> GetByGuid(Guid guid)
    {
        var status = await _statusService.GetByGuidAsync(guid);
        return status is null
            ? NotFound(Result<object>.Failure("Status não encontrado."))
            : Ok(Result<StatusDto>.Ok(status));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] StatusCreateDto statusCreateDto)
    {
        var status = await _statusService.AddAsync(statusCreateDto);
        return CreatedAtAction(nameof(GetByGuid), new { guid = status.Guid }, Result<StatusDto>.Ok(status));
    }

    [HttpPut("{guid:guid}")]
    public async Task<IActionResult> Put(Guid guid, [FromBody] StatusUpdateDto statusUpdateDto)
    {
        var status = await _statusService.UpdateAsync(guid, statusUpdateDto);
        return status is null
            ? NotFound(Result<object>.Failure("Status não encontrado."))
            : Ok(Result<StatusDto>.Ok(status));
    }

    [HttpDelete("{guid:guid}")]
    public async Task<IActionResult> Delete(Guid guid)
    {
        var deleted = await _statusService.DeleteAsync(guid);
        return deleted
            ? NoContent()
            : NotFound(Result<object>.Failure("Status não encontrado."));
    }
}
