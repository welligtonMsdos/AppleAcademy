namespace AppleAcademy.Domain.Interfaces;

public interface ICommand<T>
{
    Task<T> AddAsync(T obj);
    Task<T?> UpdateAsync(Guid guid, T obj);
    Task<bool> DeleteAsync(Guid guid);
}
