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

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURATION & OBSERVABILITY ---
builder.ConfigureSerilog();
builder.Services.AddVelyoOpenTelemetry();
builder.Services.AddVelyoRateLimiting();

// FIXED: AddNpgSql yerine EntityFramework sağlık kontrolü kullanıldı
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
app.MapHealthChecks("/health").AllowAnonymous();
app.MapVelyoHubs();
app.MapBillingEndpoints();
app.MapAuthEndpoints();
app.MapWorkspaceEndpoints().RequireAuthorization();
app.MapProjectEndpoints().RequireAuthorization();
app.MapTaskEndpoints().RequireAuthorization();
app.MapNotificationEndpoints(); // Note: Bu sınıfın içinde RequireAuthorization() tanımlanmıştı
app.MapWorkflowEndpoints().RequireAuthorization();
// --- 7. DATABASE SEEDING ---
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