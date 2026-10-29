using AppleAcademy.Domain.Entities;
using AppleAcademy.Domain.Interfaces;
using AppleAcademy.Infrastructure.Data;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace AppleAcademy.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppleAcademyContext _context;
    private readonly NpgsqlDataSource _dataSource;

    public UserRepository(AppleAcademyContext context, NpgsqlDataSource dataSource)
    {
        _context = context;
        _dataSource = dataSource;
    }

    public async Task<User> AddAsync(User obj)
    {
        await _context.Users.AddAsync(obj);
        await _context.SaveChangesAsync();
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
        const string sql = """
            SELECT u."Guid", u."Name", u."Email", u."Password", u."PerfilGuid", u."Active"
            FROM "User" u
            INNER JOIN "Perfil" p ON u."PerfilGuid" = p."Guid"
            WHERE u."Active" = true
            ORDER BY u."Name"
            """;

        using var connection = _dataSource.CreateConnection();

        return (await connection.QueryAsync<User>(sql)).ToList();
    }   
    

    public async Task<User?> GetByGuidAsync(Guid guid)
    {
        const string sql = """
            SELECT u."Guid", u."Name", u."Email", u."Password", u."PerfilGuid", u."Active"
            FROM "User" u
            INNER JOIN "Perfil" p ON u."PerfilGuid" = p."Guid"
            WHERE u."Guid" = @Guid AND u."Active" = true
            """;

        await using var connection = await _dataSource.OpenConnectionAsync();

        return await connection.QuerySingleOrDefaultAsync<User>(new CommandDefinition(sql, new { Guid = guid }));
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

        return user;
    }    
}
