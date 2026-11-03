using AppleAcademy.Domain.Entities;
using AppleAcademy.Domain.Interfaces;
using AppleAcademy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AppleAcademy.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppleAcademyContext _context;
    public UserRepository(AppleAcademyContext context)
    {
        _context = context;
    }

    public async Task<User> AddAsync(User obj)
    {
        await _context.Users.AddAsync(obj);
        await _context.SaveChangesAsync();
        await _context.Entry(obj).Reference(u => u.Perfil).LoadAsync();
        return obj;
    }

    public async Task<bool> DeleteAsync(Guid guid)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Guid == guid && u.Active);
        if (user is null) return false;
        user.Active = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<ICollection<User>> GetAllAsync()
    {
        return await _context.Users.Include(u => u.Perfil)
            .Where(u => u.Active).OrderBy(u => u.Name).ToListAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.Include(u => u.Perfil)
            .FirstOrDefaultAsync(u => u.Email == email && u.Active);
    }

    public async Task<User?> GetByGuidAsync(Guid guid)
    {
        return await _context.Users.Include(u => u.Perfil)
            .FirstOrDefaultAsync(u => u.Guid == guid && u.Active);
    }

    public async Task<User?> UpdateAsync(Guid guid, User obj)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Guid == guid && u.Active);

        if (user is null) return null;

        user.Name = obj.Name;
        user.Email = obj.Email;
        user.Password = obj.Password;
        user.PerfilGuid = obj.PerfilGuid;

        await _context.SaveChangesAsync();
        await _context.Entry(user).Reference(u => u.Perfil).LoadAsync();

        return user;
    }    
}
