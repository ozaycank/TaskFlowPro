using MediatR;

namespace Velyo.Application.Documents.Queries.GetDocumentTree;

public record GetDocumentTreeQuery(Guid WorkspaceId, Guid? ProjectId) : IRequest<IEnumerable<DocumentDto>>;