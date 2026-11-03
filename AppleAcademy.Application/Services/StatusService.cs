using AppleAcademy.Application.Dtos;
using AppleAcademy.Application.Interfaces;
using AppleAcademy.Domain.Entities;
using AppleAcademy.Domain.Interfaces;

namespace AppleAcademy.Application.Services;

public class StatusService : IStatusService
{
    private readonly IStatusRepository _repository;

    public StatusService(IStatusRepository repository)
    {
        _repository = repository;
    }

    public async Task<StatusDto> AddAsync(StatusCreateDto statusCreateDto)
    {
        ArgumentNullException.ThrowIfNull(statusCreateDto);

        var status = new Status 
        {
            Guid = Guid.NewGuid(),
            Description = statusCreateDto.Description,
            Active = true
        };

        await _repository.AddAsync(status);

        return ToDto(status);
    }

    public async Task<bool> DeleteAsync(Guid guid)
    {
        if (guid == Guid.Empty) return false;

        return await _repository.DeleteAsync(guid);
    }

    public async Task<ICollection<StatusDto>> GetAllAsync()
    {
        return (await _repository.GetAllAsync()).Select(ToDto).ToList();
    }

    public async Task<StatusDto?> GetByGuidAsync(Guid guid)
    {
        if (guid == Guid.Empty) return null;

        var status = await _repository.GetByGuidAsync(guid);

        return status is null ? null : ToDto(status);
    }

    public async Task<StatusDto?> UpdateAsync(Guid guid, StatusUpdateDto statusUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(statusUpdateDto);

        if (guid == Guid.Empty) return null;

        var status = new Status
        {
            Guid = guid,
            Description = statusUpdateDto.Description
        };

        var atualizado = await _repository.UpdateAsync(guid, status);

        return atualizado is null ? null : ToDto(atualizado);
    }

    private static StatusDto ToDto(Status status) => new(
        status.Guid,
        status.Description!);
}
