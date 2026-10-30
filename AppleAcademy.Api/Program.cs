using AppleAcademy.Api.Filters;
using AppleAcademy.Api.Middleware;
using AppleAcademy.Application.Extensions;
using AppleAcademy.Infrastructure.Data;
using Microsoft.AspNetCore.HttpOverrides;
using Scalar.AspNetCore;

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

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidatorFilter>();
});

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();

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

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();