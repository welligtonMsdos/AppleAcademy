namespace AppleAcademy.Application.Dtos;

public record StatusDto(Guid Guid, 
                        string Description);

public record StatusCreateDto(string Description);

public record StatusUpdateDto(Guid Guid,
                              string Description);
