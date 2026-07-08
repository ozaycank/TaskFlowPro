using MediatR;
using Velyo.Application.Documents.Commands.CreateDocument;
using Velyo.Application.Documents.Commands.DeleteDocument;
using Velyo.Application.Documents.Commands.UpdateDocument;
using Velyo.Application.Documents.Queries.GetDocumentById;
using Velyo.Application.Documents.Queries.GetDocumentTree;

namespace Velyo.Api.Endpoints;

public static class DocumentEndpoints
{
    public static RouteGroupBuilder MapDocumentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/documents").WithTags("Documents").RequireAuthorization();

        group.MapGet("/workspaces/{workspaceId:guid}", async (Guid workspaceId, Guid? projectId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetDocumentTreeQuery(workspaceId, projectId));
            return Results.Ok(result);
        }).WithName("GetDocumentTree").WithOpenApi();

        group.MapGet("/{documentId:guid}", async (Guid documentId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetDocumentByIdQuery(documentId));
            return Results.Ok(result);
        }).WithName("GetDocumentById").WithOpenApi();

        group.MapPost("/", async (CreateDocumentCommand command, IMediator mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/api/documents/{id}", id);
        }).WithName("CreateDocument").WithOpenApi();

        group.MapPut("/{documentId:guid}", async (Guid documentId, UpdateDocumentCommand command, IMediator mediator) =>
        {
            if (documentId != command.DocumentId) return Results.BadRequest("Document ID mismatch");
            await mediator.Send(command);
            return Results.NoContent();
        }).WithName("UpdateDocument").WithOpenApi();

        group.MapDelete("/{documentId:guid}", async (Guid documentId, IMediator mediator) =>
        {
            await mediator.Send(new DeleteDocumentCommand(documentId));
            return Results.NoContent();
        }).WithName("DeleteDocument").WithOpenApi();

        return group;
    }
}