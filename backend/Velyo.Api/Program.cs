using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Velyo.Api.Configuration;
using Velyo.Api.Endpoints;
using Velyo.Api.Extensions;
using Velyo.Api.Middleware;
using Velyo.Api.Services;
using Velyo.Application;
using Velyo.Application.Auth.Services;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Infrastructure;
using Velyo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Velyo.Infrastructure.Persistence.Seeder;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURATION & OBSERVABILITY ---
builder.ConfigureSerilog();
builder.Services.AddVelyoOpenTelemetry();
builder.Services.AddVelyoRateLimiting();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>("Database");

// --- 2. CORE API SERVICES ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddVelyoSwagger();
builder.Services.AddVelyoCors(builder.Configuration);

// --- 3. SECURITY ---
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

// --- 4. DEPENDENCY INJECTION ---
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

// --- 5. HTTP REQUEST PIPELINE (Strict Ordering) ---
app.UseExceptionHandler(); // En üstte olmalı: Tüm middleware hatalarını yakalamak için

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging(); // İstekleri çok erken loglamak için
app.UseHttpsRedirection();
app.UseCors(CorsExtensions.VelyoCorsPolicy);
app.UseRateLimiter(); // DDoS koruması kimlik doğrulamadan önce çalışmalı

app.UseAuthentication();
app.UseAuthorization();

// Custom Middleware MUST be after Auth to access user claims
app.UseMiddleware<RequestLogContextMiddleware>();

// --- 6. ENDPOINT MAPPING ---
// Public Endpoints
app.MapHealthChecks("/health").AllowAnonymous();
app.MapAuthEndpoints();
app.MapBillingEndpoints(); // Webhook'lar içerebileceği için auth kendi içinde yönetilir
app.MapVelyoHubs();        // SignalR token auth'unu kendi içinde yönetir

// Protected Business Endpoints (Güvenlik ağı olarak hepsine .RequireAuthorization() eklendi)
app.MapWorkspaceEndpoints().RequireAuthorization();
app.MapProjectEndpoints().RequireAuthorization();
app.MapTaskEndpoints().RequireAuthorization();
app.MapSprintEndpoints().RequireAuthorization();
app.MapWorkflowEndpoints().RequireAuthorization();
app.MapCustomFieldEndpoints().RequireAuthorization();
app.MapActivityEndpoints().RequireAuthorization();
app.MapSearchEndpoints().RequireAuthorization();
app.MapDocumentEndpoints().RequireAuthorization();
app.MapNotificationEndpoints().RequireAuthorization();
app.MapAutomationEndpoints().RequireAuthorization();
app.MapAnalyticsEndpoints().RequireAuthorization();

// --- 7. DATABASE MIGRATION & SEEDING ---
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        await context.Database.MigrateAsync();
        await ApplicationDbContextSeeder.SeedSampleDataAsync(context, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}

app.Run();