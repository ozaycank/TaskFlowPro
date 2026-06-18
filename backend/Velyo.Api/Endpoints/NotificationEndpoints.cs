using MediatR;
using Velyo.Application.Notifications.Commands.MarkAllAsRead;
using Velyo.Application.Notifications.Commands.MarkAsRead;
using Velyo.Application.Notifications.Queries.GetMyNotifications;

namespace Velyo.Api.Endpoints;

public static class NotificationEndpoints
{
    public static RouteGroupBuilder MapNotificationEndpoints(this IEndpointRouteBuilder app)
    {
        // RequireAuthorization() eklenerek tüm bildirim uç noktaları güvence altına alınır
        var group = app.MapGroup("/api/notifications").WithTags("Notifications").RequireAuthorization();

        // GET /api/notifications?unreadOnly=true
        group.MapGet("/", async (bool? unreadOnly, IMediator mediator) =>
        {
            var query = new GetMyNotificationsQuery(unreadOnly ?? false);
            var notifications = await mediator.Send(query);
            return Results.Ok(notifications);
        })
        .WithName("GetMyNotifications")
        .WithOpenApi();

        // PUT /api/notifications/{id}/read
        group.MapPut("/{notificationId:guid}/read", async (Guid notificationId, IMediator mediator) =>
        {
            await mediator.Send(new MarkNotificationAsReadCommand(notificationId));
            return Results.NoContent();
        })
        .WithName("MarkNotificationAsRead")
        .WithOpenApi();

        // PUT /api/notifications/read-all
        group.MapPut("/read-all", async (IMediator mediator) =>
        {
            await mediator.Send(new MarkAllNotificationsAsReadCommand());
            return Results.NoContent();
        })
        .WithName("MarkAllNotificationsAsRead")
        .WithOpenApi();

        return group;
    }
}