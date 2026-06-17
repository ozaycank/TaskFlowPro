namespace Velyo.Api.Extensions;

public static class CorsExtensions
{
    public const string VelyoCorsPolicy = "VelyoCorsPolicy";

    public static IServiceCollection AddVelyoCors(this IServiceCollection services, IConfiguration configuration)
    {
        // In a real scenario, allowed origins would be fetched from appsettings.json
        var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>()
                             ?? new[] { "http://localhost:3000", "http://localhost:5173" }; // Defaults for React/Vite

        services.AddCors(options =>
        {
            options.AddPolicy(VelyoCorsPolicy, builder =>
            {
                builder.WithOrigins(allowedOrigins)
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials(); // Required if you use cookies or auth headers later
            });
        });

        return services;
    }
}