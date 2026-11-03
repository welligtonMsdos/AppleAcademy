using AppleAcademy.Application.Dtos;
using AppleAcademy.Application.Exceptions;
using AppleAcademy.Application.Interfaces;
using AppleAcademy.Domain.Entities;
using AppleAcademy.Domain.Interfaces;

namespace AppleAcademy.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly ITokenService _tokenService;

    public UserService(IUserRepository repository, 
                       ITokenService tokenService)
    {
        _repository = repository;
        _tokenService = tokenService;
    }

    public async Task<UserDto> AddAsync(UserCreateDto userCreateDto)
    {
        ArgumentNullException.ThrowIfNull(userCreateDto);

        var user = new User
        {
            Guid = Guid.NewGuid(),
            Name = userCreateDto.Name,
            Email = userCreateDto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(userCreateDto.Password),
            Active = true,
            PerfilGuid = userCreateDto.PerfilGuid
        };

        await _repository.AddAsync(user);

        return ToDto(user);
    }

    public async Task<bool> DeleteAsync(Guid guid)
    {
        if (guid == Guid.Empty) throw new ArgumentException("O identificador do Usuário é inválido.", nameof(guid));

        return await _repository.DeleteAsync(guid);
    }

    public async Task<ICollection<UserDto>> GetAllAsync()
    {
        return (await _repository.GetAllAsync()).Select(ToDto).ToList();
    }

    public async Task<UserDto?> GetByGuidAsync(Guid guid)
    {
        if (guid == Guid.Empty) throw new ArgumentException("O identificador do Usuário é inválido.", nameof(guid));

        var user = await _repository.GetByGuidAsync(guid);

        return user is null ? null : ToDto(user);
    }

    public async Task<UserDto?> UpdateAsync(Guid guid, UserUpdateDto userUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(userUpdateDto);

        if (guid == Guid.Empty) throw new ArgumentException("O identificador do Usuário é inválido.", nameof(guid));

        var userExists = await _repository.GetByGuidAsync(guid);

        if (userExists is null) throw new InvalidOperationException("O Usuário não foi encontrado.");

        var emailExists = await _repository.GetByEmailAsync(userUpdateDto.Email);
        if (emailExists is not null && emailExists.Guid != guid)
            throw new BusinessException("O e-mail informado já está em uso por outro usuário.");

        userExists.Name = userUpdateDto.Name;
        userExists.Email = userUpdateDto.Email;
        userExists.PerfilGuid = userUpdateDto.PerfilGuid;
        userExists.Active = userExists.Active;        

        var atualizado = await _repository.UpdateAsync(guid, userExists);

        return atualizado is null ? null : ToDto(atualizado);
    }

    private static UserDto ToDto(User user) => new(
        user.Guid,
        user.Name!,
        user.Email!,
        user.PerfilGuid,
        user.Perfil!.Name!);

    public async Task<string> LoginAsync(UserLoginDto userLoginDto)
    {
        var user = await _repository.GetByEmailAsync(userLoginDto.Email);

        if (user is null) throw new BusinessException("Usuário não encontrado.");

        if (!BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.Password))
            throw new BusinessException("Senha inválida.");

        var userDataLoginDto = new UserDataLoginDto(
            user.Guid, 
            user.Name!, 
            user.Email!, 
            user.Perfil!.Name!);

        var token = await _tokenService.GenerateToken(userDataLoginDto);

        return token;
    }
}
