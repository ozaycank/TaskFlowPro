using MediatR;
using Microsoft.AspNetCore.Mvc;
using Velyo.Application.Workspaces.Commands.CreateWorkspace;
using Velyo.Application.Workspaces.Queries.GetWorkspaces;
using Velyo.Application.Workspaces.Commands.InviteMember;
using Velyo.Application.Workspaces.Commands.AcceptInvitation;
using Velyo.Application.Workspaces.Commands.RemoveMember;
using Velyo.Application.Workspaces.Queries.GetWorkspaceById;
using Velyo.Application.Workspaces.Commands.UpdateWorkspace;
using Velyo.Application.Workspaces.Commands.DeleteWorkspace;

namespace Velyo.Api.Endpoints;

public static class WorkspaceEndpoints
{
    public static RouteGroupBuilder MapWorkspaceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/workspaces").WithTags("Workspaces");

        // POST Endpoint (Existing)
        group.MapPost("/", async (CreateWorkspaceCommand command, IMediator mediator) =>
        {
            var workspaceId = await mediator.Send(command);
            return Results.Created($"/api/workspaces/{workspaceId}", workspaceId);
        })
        .WithName("CreateWorkspace")
        .WithOpenApi();

        group.MapGet("/", async (IMediator mediator) =>
        {
            var query = new GetWorkspacesQuery();
            var workspaces = await mediator.Send(query);
            return Results.Ok(workspaces);
        })
        .WithName("GetWorkspaces")
        .WithOpenApi();

        // FIX: Frontend '/invite' rotasını arıyor, adres '/invite' olarak düzeltildi.
        // POST /api/workspaces/{workspaceId}/invite
        group.MapPost("/{workspaceId:guid}/invite", async (Guid workspaceId, [FromBody] InviteMemberCommand command, IMediator mediator) =>
        {
            // Frontend'den gelen payload içinde WorkspaceId yoksa rotadakini ekliyoruz.
            if (command.WorkspaceId == Guid.Empty)
            {
                command = command with { WorkspaceId = workspaceId };
            }
            else if (workspaceId != command.WorkspaceId)
            {
                return Results.BadRequest("Route WorkspaceId does not match Body WorkspaceId.");
            }

            await mediator.Send(command);

            // SAAS SECURITY: Token artık HTTP isteğinde dönmüyor.
            return Results.Ok(new { Message = "Invitation sent successfully to the provided email address." });
        })
        .WithName("InviteWorkspaceMember")
        .WithOpenApi();

        // POST /api/workspaces/invitations/accept
        group.MapPost("/invitations/accept", async (AcceptInvitationCommand command, IMediator mediator) =>
        {
            await mediator.Send(command);
            return Results.Ok(new { Message = "Successfully joined the workspace." });
        });

        // GET Workspace Members
        group.MapGet("/{workspaceId:guid}/members", (Guid workspaceId, IMediator mediator) =>
        {
            // Şimdilik boş liste dönüyor, ileride GetWorkspaceMembersQuery bağlanacak
            return Results.Ok(new List<object>());
        })
        .WithName("GetWorkspaceMembers")
        .WithOpenApi();

        // DELETE /api/workspaces/{workspaceId}/members/{userId}
        group.MapDelete("/{workspaceId:guid}/members/{userId:guid}", async (Guid workspaceId, Guid userId, IMediator mediator) =>
        {
            await mediator.Send(new RemoveMemberCommand(workspaceId, userId));
            return Results.NoContent();
        });

        // GET /api/workspaces/{workspaceId}
        group.MapGet("/{workspaceId:guid}", async (Guid workspaceId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetWorkspaceByIdQuery(workspaceId));
            return Results.Ok(result);
        })
        .WithName("GetWorkspaceById")
        .WithOpenApi();

        // PUT /api/workspaces/{workspaceId}
        group.MapPut("/{workspaceId:guid}", async (Guid workspaceId, UpdateWorkspaceCommand command, IMediator mediator) =>
        {
            if (workspaceId != command.WorkspaceId) return Results.BadRequest("Workspace ID mismatch");
            await mediator.Send(command);
            return Results.NoContent();
        })
        .WithName("UpdateWorkspace")
        .WithOpenApi();

        // DELETE /api/workspaces/{workspaceId}
        group.MapDelete("/{workspaceId:guid}", async (Guid workspaceId, IMediator mediator) =>
        {
            await mediator.Send(new DeleteWorkspaceCommand(workspaceId));
            return Results.NoContent();
        })
        .WithName("DeleteWorkspace")
        .WithOpenApi();

        return group;
    }
}