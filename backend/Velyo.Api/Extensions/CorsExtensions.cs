namespace Velyo.Api.Extensions; // Dikkat: Namespace Extensions olmalı

public static class CorsExtensions
{
    public const string VelyoCorsPolicy = "VelyoCorsPolicy";

    public static IServiceCollection AddVelyoCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(VelyoCorsPolicy, builder =>
            {
                // Frontend URL'sine açıkça izin ver (127.0.0.1 ve localhost)
                builder.WithOrigins("http://localhost:3000", "http://127.0.0.1:3000")
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials()
                       .WithExposedHeaders("Token-Expired");
            });
        });

        return services;
    }
}