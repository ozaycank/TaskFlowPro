using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Velyo.Api.Endpoints;
using Velyo.Api.Extensions;
using Velyo.Api.Middleware;
using Velyo.Api.Services;
using Velyo.Application;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Infrastructure;
using Velyo.Infrastructure.Persistence;

// FIXED: Eksik olan namespace eklendi (IPasswordHasher ve ITokenService için)
using Velyo.Application.Auth.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddVelyoSwagger();
builder.Services.AddVelyoCors(builder.Configuration);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, Velyo.Infrastructure.Identity.CurrentUserService>();
builder.Services.AddScoped<IPasswordHasher, Velyo.Infrastructure.Identity.PasswordHasher>();
builder.Services.AddScoped<ITokenService, Velyo.Infrastructure.Identity.TokenService>();
builder.Services.AddScoped<IWorkspaceAuthorizationService, Velyo.Infrastructure.Identity.WorkspaceAuthorizationService>();
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(CorsExtensions.VelyoCorsPolicy);

// Authentication ve Authorization MUST be after Cors and before Endpoints
app.UseAuthentication();
app.UseAuthorization();
app.MapNotificationEndpoints();
app.UseExceptionHandler();

// FIXED: Çifte Endpoint mapping kaldırıldı, sadece RequireAuthorization eklendi
app.MapAuthEndpoints(); // Auth endpointleri herkese açık olmalı (Login/Register)
app.MapWorkspaceEndpoints().RequireAuthorization(); // JWT ile korunuyor
app.MapProjectEndpoints().RequireAuthorization();   // JWT ile korunuyor
app.MapTaskEndpoints().RequireAuthorization();      // JWT ile korunuyor

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await DatabaseSeeder.SeedDevelopmentDataAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();