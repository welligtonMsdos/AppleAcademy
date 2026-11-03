using AppleAcademy.Application.Dtos;
using AppleAcademy.Application.Interfaces;
using AppleAcademy.Domain.Entities;
using AppleAcademy.Domain.Interfaces;

namespace AppleAcademy.Application.Services;

public class PerfilService : IPerfilService
{
    private readonly IPerfilRepository _repository;

    public PerfilService(IPerfilRepository repository)
    {
        _repository = repository;
    }

    public async Task<PerfilDto> AddAsync(PerfilCreateDto perfilCreateDto)
    {
        ArgumentNullException.ThrowIfNull(perfilCreateDto);

        var perfil = new Perfil
        {
            Guid = Guid.NewGuid(),
            Name = perfilCreateDto.Name,
            Active = true
        };

        await _repository.AddAsync(perfil);

        return ToDto(perfil);
    }

    public async Task<bool> DeleteAsync(Guid guid)
    {
        if (guid == Guid.Empty) throw new ArgumentException("O identificador do Perfil é inválido.", nameof(guid));

        return await _repository.DeleteAsync(guid);
    }

    public async Task<ICollection<PerfilDto>> GetAllAsync()
    {
        return (await _repository.GetAllAsync()).Select(ToDto).ToList();
    }

    public async Task<PerfilDto?> GetByGuidAsync(Guid guid)
    {
        if (guid == Guid.Empty) throw new ArgumentException("O identificador do Perfil é inválido.", nameof(guid));

        var perfil = await _repository.GetByGuidAsync(guid);

        return perfil is null ? null : ToDto(perfil);
    }

    public async Task<PerfilDto?> UpdateAsync(Guid guid, PerfilUpdateDto perfilUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(perfilUpdateDto);

        if (guid == Guid.Empty) throw new ArgumentException("O identificador do Perfil é inválido.", nameof(guid));

        var perfil = new Perfil
        {
            Guid = guid,
            Name = perfilUpdateDto.Name
        };

        var atualizado = await _repository.UpdateAsync(guid, perfil);

        return atualizado is null ? null : ToDto(atualizado);
    }

    private static PerfilDto ToDto(Perfil perfil) => new(
        perfil.Guid,
        perfil.Name!);    
}
