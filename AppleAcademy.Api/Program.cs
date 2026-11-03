using AppleAcademy.Api.Filters;
using AppleAcademy.Api.Middleware;
using AppleAcademy.Application.Extensions;
using AppleAcademy.Infrastructure.Data;
using AppleAcademy.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

if (File.Exists(".env"))
    DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var secret = builder.Configuration["JWT_KEY"];

if (string.IsNullOrEmpty(secret) || secret.Length < 32)
    throw new InvalidOperationException("JWT key is missing or too short (minimum 32 characters).");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ValidateIssuer = true,
            ValidIssuer = "http://localhost:5000",
            ValidateAudience = false,
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidatorFilter>();
});

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();

{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppleAcademyContext>();
    await db.Database.MigrateAsync();

    var defaultProfiles = new[]
    {
        (Guid: AppleAcademyContext.AdminPerfilGuid, Name: "Admin"),
        (Guid: AppleAcademyContext.AlunoPerfilGuid, Name: "Aluno"),
        (Guid: AppleAcademyContext.ProfessorPerfilGuid, Name: "Professor")
    };
    foreach (var profile in defaultProfiles)
    {
        if (!await db.Perfis.AnyAsync(p => EF.Functions.ILike(p.Name!, profile.Name)))
            db.Perfis.Add(new Perfil { Guid = profile.Guid, Name = profile.Name, Active = true });
    }
    await db.SaveChangesAsync();

    var defaultStatuses = new[]
    {
        (Guid: AppleAcademyContext.PendenteStatusGuid, Description: "Pendente"),
        (Guid: AppleAcademyContext.ConcluidoStatusGuid, Description: "Concluído"),
        (Guid: AppleAcademyContext.NegadoStatusGuid, Description: "Negado")
    };
    foreach (var defaultStatus in defaultStatuses)
    {
        if (!await db.Statuses.AnyAsync(s => EF.Functions.ILike(s.Description!, defaultStatus.Description)))
            db.Statuses.Add(new Status { Guid = defaultStatus.Guid, Description = defaultStatus.Description, Active = true });
    }
    await db.SaveChangesAsync();

    var adminEmail = app.Configuration["ADMIN_EMAIL"];
    var adminPassword = app.Configuration["ADMIN_PASSWORD"];
    if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword) || adminPassword.Length < 10)
        throw new InvalidOperationException("Configure ADMIN_EMAIL e ADMIN_PASSWORD (mínimo de 10 caracteres) para criar o administrador inicial.");

    if (!await db.Users.AnyAsync(u => u.Active && EF.Functions.ILike(u.Perfil!.Name!, "Admin")))
    {
        if (await db.Users.AnyAsync(u => u.Email == adminEmail))
            throw new InvalidOperationException("ADMIN_EMAIL já está em uso por outro usuário.");

        var adminPerfilGuid = await db.Perfis.Where(p => EF.Functions.ILike(p.Name!, "Admin"))
            .Select(p => p.Guid).FirstAsync();
        db.Users.Add(new User
        {
            Guid = Guid.NewGuid(),
            Name = "Administrador",
            Email = adminEmail,
            Password = BCrypt.Net.BCrypt.HashPassword(adminPassword),
            PerfilGuid = adminPerfilGuid,
            Active = true
        });
        await db.SaveChangesAsync();
    }
}

app.UseForwardedHeaders();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapOpenApi();

app.MapScalarApiReference(options =>
{
    options.Title = "Apple Academy API Reference";
    options.Theme = ScalarTheme.BluePlanet;
    options.DefaultHttpClient = new(ScalarTarget.JavaScript, ScalarClient.HttpClient);
    options.CustomCss = "";
    options.ShowSidebar = true;
    options.DarkMode = true;
    options.AddPreferredSecuritySchemes("Bearer")
           .AddHttpAuthentication("Bearer", auth =>
           {
               auth.Token = "your-bearer-token";
           });
});

app.UseCors("CorsPolicy");

if (!string.IsNullOrWhiteSpace(app.Configuration["HTTPS_PORTS"]) ||
    !string.IsNullOrWhiteSpace(app.Configuration["HTTPS_PORT"]))
    app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
