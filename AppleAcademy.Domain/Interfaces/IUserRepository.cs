using AppleAcademy.Domain.Entities;

namespace AppleAcademy.Domain.Interfaces;

public interface IUserRepository: IRead<User>, ICommand<User>
{
    Task<User?> GetByEmailAsync(string email);
}
