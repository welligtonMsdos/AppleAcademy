namespace AppleAcademy.Domain.Entities;

public class User
{
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool Active { get; set; }
    public Guid PerfilGuid { get; set; }
    public Perfil Perfil { get; set; }
}
