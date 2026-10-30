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
