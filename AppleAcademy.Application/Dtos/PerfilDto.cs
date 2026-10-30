namespace AppleAcademy.Application.Dtos;

public record PerfilDto(Guid Guid, 
                        string Name);

public record PerfilCreateDto(string Name);

public record PerfilUpdateDto(Guid Guid,
                              string Name);
