using AppleAcademy.Domain.Entities;
using AppleAcademy.Domain.Interfaces;
using AppleAcademy.Infrastructure.Data;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace AppleAcademy.Infrastructure.Repositories;

public class PerfilRepository : IPerfilRepository
{
    private readonly AppleAcademyContext _context;
    private readonly NpgsqlDataSource _dataSource;

    public PerfilRepository(AppleAcademyContext context, NpgsqlDataSource dataSource)
    {
        _context = context;
        _dataSource = dataSource;
    }

    public async Task<Perfil> AddAsync(Perfil obj)
    {
        await _context.Perfis.AddAsync(obj);
        await _context.SaveChangesAsync();
        return obj;
    }

    public async Task<bool> DeleteAsync(Guid guid)
    {
        var perfil = await _context.Perfis.FirstOrDefaultAsync(p => p.Guid == guid && p.Active);
        if (perfil is null) return false;
        perfil.Active = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<ICollection<Perfil>> GetAllAsync()
    {
        const string sql = """
            SELECT "Guid", "Name", "Active"
            FROM "Perfil"
            WHERE "Active" = true
            ORDER BY "Name"
            """;

        using var connection = _dataSource.CreateConnection();

        return (await connection.QueryAsync<Perfil>(sql)).ToList();
    }
    

    public async Task<Perfil?> GetByGuidAsync(Guid guid)
    {
        const string sql = """
            SELECT "Guid", "Name", "Active"
            FROM "Perfil"
            WHERE "Guid" = @Guid AND "Active" = true
            """;

        await using var connection = await _dataSource.OpenConnectionAsync();

        return await connection.QuerySingleOrDefaultAsync<Perfil>(new CommandDefinition(sql, new { Guid = guid }));
    }

    public async Task<Perfil?> UpdateAsync(Guid guid, Perfil obj)
    {
        var perfil = await _context.Perfis.FirstOrDefaultAsync(p => p.Guid == guid && p.Active);

        if (perfil is null) return null;

        perfil.Name = obj.Name;      

        await _context.SaveChangesAsync();

        return perfil;
    }
}
