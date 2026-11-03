namespace AppleAcademy.Application.Dtos;

public record UserDto(Guid Guid,
                      string Name,
                      string Email,
                      Guid PerfilGuid,
                      string PerfilName);

public record UserCreateDto(string Name,
                            string Email,
                            string Password,
                            Guid PerfilGuid);

public record UserUpdateDto(Guid Guid,
                            string Name,
                            string Email,
                            Guid PerfilGuid);

public record UserDataLoginDto(Guid Guid,
                               string Name,
                               string Email,
                               string PerfilName);

public record UserLoginDto(string Email,
                           string Password);
