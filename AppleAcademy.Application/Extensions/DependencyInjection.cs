using AppleAcademy.Application.Interfaces;
using AppleAcademy.Application.Services;
using AppleAcademy.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AppleAcademy.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPerfilService, PerfilService>();
        services.AddScoped<IStatusService, StatusService>();
        services.AddScoped<IUserService, UserService>();

        services.AddValidatorsFromAssembly(typeof(CreatePerfilValidator).Assembly);

        return services;
    }
}
