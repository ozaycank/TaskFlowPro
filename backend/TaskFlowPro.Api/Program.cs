using TaskFlowPro.Api.Endpoints;
using TaskFlowPro.Api.Middleware;
using TaskFlowPro.Api.Services;
using TaskFlowPro.Application;
using TaskFlowPro.Application.Common.Interfaces.Services;
using TaskFlowPro.Infrastructure;
using TaskFlowPro.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Dependency Injection ---

// Core Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// API Layer Specific Services
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<ICurrentUserService, DevelopmentCurrentUserService>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Add Katmanlar (Layers)
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

// Use the GlobalExceptionHandler
app.UseExceptionHandler();

// Map Endpoints
app.MapWorkspaceEndpoints();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        // Veritabanının oluşturulduğundan emin ol ve tohumla
        await DatabaseSeeder.SeedDevelopmentDataAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();