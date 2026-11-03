using AppleAcademy.Application.Dtos;

namespace AppleAcademy.Application.Interfaces;

public interface IStatusService
{
    Task<StatusDto> AddAsync(StatusCreateDto statusCreateDto);
    Task<StatusDto?> UpdateAsync(Guid guid, StatusUpdateDto statusUpdateDto);
    Task<bool> DeleteAsync(Guid guid);
    Task<ICollection<StatusDto>> GetAllAsync();
    Task<StatusDto?> GetByGuidAsync(Guid guid);
}
