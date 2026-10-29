using AppleAcademy.Domain.Entities;
using AppleAcademy.Domain.Interfaces;
using AppleAcademy.Infrastructure.Data;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace AppleAcademy.Infrastructure.Repositories;

public class StatusRepository : IStatusRepository
{
    private readonly AppleAcademyContext _context;
    private readonly NpgsqlDataSource _dataSource;

    public StatusRepository(AppleAcademyContext context, NpgsqlDataSource dataSource)
    {
        _context = context;
        _dataSource = dataSource;
    }

    public async Task<Status> AddAsync(Status obj)
    {
        await _context.Statuses.AddAsync(obj);
        await _context.SaveChangesAsync();
        return obj;
    }

    public async Task<bool> DeleteAsync(Guid guid)
    {
        var status = await _context.Statuses.FirstOrDefaultAsync(s => s.Guid == guid && s.Active);
        if (status is null) return false;
        status.Active = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<ICollection<Status>> GetAllAsync()
    {
        const string sql = """
            SELECT "Guid", "Name", "Description", "Active"
            FROM "Status" 
            WHERE "Active" = true
            ORDER BY "Name"
            """;
            
        using var connection = _dataSource.CreateConnection();

        return (await connection.QueryAsync<Status>(sql)).ToList();
    }

    public async Task<Status?> GetByGuidAsync(Guid guid)
    {
        const string sql = """
            SELECT "Guid", "Name", "Description", "Active"
            FROM "Departamento"
            WHERE "Guid" = @Guid AND "Ativo" = TRUE;
            """;

        await using var connection = await _dataSource.OpenConnectionAsync();

        return await connection.QuerySingleOrDefaultAsync<Status>(new CommandDefinition(sql, new { Guid = guid }));
    }

    public async Task<Status?> UpdateAsync(Guid guid, Status obj)
    {
        var currentStatus = await _context.Statuses.FirstOrDefaultAsync(
             current => current.Guid == guid && current.Active);

        if (currentStatus is null) return null;
       
        currentStatus.Description = obj.Description;      

        await _context.SaveChangesAsync();

        return currentStatus;
    }
}
