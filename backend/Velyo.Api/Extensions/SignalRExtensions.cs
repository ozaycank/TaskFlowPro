using Velyo.Infrastructure.RealTime;

namespace Velyo.Api.Extensions;

public static class SignalRExtensions
{
    public static void MapVelyoHubs(this IEndpointRouteBuilder app)
    {
        app.MapHub<VelyoHub>("/hubs/velyo");
    }
}