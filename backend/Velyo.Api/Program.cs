using Velyo.Api.Endpoints;
using Velyo.Api.Extensions;
using Velyo.Api.Middleware;
using Velyo.Api.Services;
using Velyo.Application;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Infrastructure;
using Velyo.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Dependency Injection ---

// Core Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddVelyoSwagger(); // Extracted Swagger config
builder.Services.AddVelyoCors(builder.Configuration); // Extracted CORS config

// API Layer Specific Services
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<ICurrentUserService, DevelopmentCurrentUserService>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(); // RFC 7807 Support

// Add Layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// --- 2. HTTP Request Pipeline ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// IMPORTANT: CORS must be placed before UseAuthentication and UseAuthorization
app.UseCors(CorsExtensions.VelyoCorsPolicy);

// Global Exception Handler
app.UseExceptionHandler();

// Map Minimal API Endpoints
app.MapWorkspaceEndpoints();
app.MapProjectEndpoints();
app.MapTaskEndpoints();

// --- DEVELOPMENT DATA SEEDING BLOCK ---
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