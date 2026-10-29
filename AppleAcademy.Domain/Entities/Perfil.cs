namespace AppleAcademy.Domain.Entities;

public class Perfil
{
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public bool Active { get; set; }
    public ICollection<User> Users { get; set; }
}
