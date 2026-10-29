namespace AppleAcademy.Domain.Entities;

public class Status
{
    public Guid Guid { get; set; }   
    public string? Description { get; set; }
    public bool Active { get; set; }
}
