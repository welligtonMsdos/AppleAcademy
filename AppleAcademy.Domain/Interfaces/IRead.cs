namespace AppleAcademy.Domain.Interfaces;

public interface IRead<T>
{
    Task<ICollection<T>> GetAllAsync();
    Task<T?> GetByGuidAsync(Guid guid);
}
