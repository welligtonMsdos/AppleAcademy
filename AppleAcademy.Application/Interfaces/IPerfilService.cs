using AppleAcademy.Application.Dtos;

namespace AppleAcademy.Application.Interfaces;

public interface IPerfilService
{
    Task<PerfilDto> AddAsync(PerfilCreateDto perfilCreateDto);
    Task<PerfilDto?> UpdateAsync(Guid guid, PerfilUpdateDto perfilUpdateDto);
    Task<bool> DeleteAsync(Guid guid);
    Task<ICollection<PerfilDto>> GetAllAsync();
    Task<PerfilDto?> GetByGuidAsync(Guid guid);
}
