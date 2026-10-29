using AppleAcademy.Domain.Interfaces;
using AppleAcademy.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace AppleAcademy.Infrastructure.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AppleAcademyConnection")
            ?? throw new InvalidOperationException("A conexão PostgreSQL 'ConnectionStrings:AppleAcademyConnection' não foi configurada.");

        services.AddDbContext<AppleAcademyContext>(options => options.UseNpgsql(connectionString));       

        services.AddSingleton<NpgsqlDataSource>(_ => NpgsqlDataSource.Create(connectionString));

        services.AddScoped<IPerfilRepository, PerfilRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IStatusRepository, StatusRepository>();

        return services;
    }
}
