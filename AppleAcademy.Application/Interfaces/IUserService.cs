using AppleAcademy.Application.Dtos;

namespace AppleAcademy.Application.Interfaces;

public interface IUserService
{
    Task<UserDto> AddAsync(UserCreateDto userCreateDto);
    Task<UserDto?> UpdateAsync(Guid guid, UserUpdateDto userUpdateDto);
    Task<bool> DeleteAsync(Guid guid);
    Task<ICollection<UserDto>> GetAllAsync();
    Task<UserDto?> GetByGuidAsync(Guid guid);
}
