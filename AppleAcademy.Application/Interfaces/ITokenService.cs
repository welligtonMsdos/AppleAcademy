using AppleAcademy.Application.Dtos;

namespace AppleAcademy.Application.Interfaces;

public interface ITokenService
{
    Task<string> GenerateToken(UserDataLoginDto userDataLoginDto);
}
