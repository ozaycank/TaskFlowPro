using MediatR;
namespace Velyo.Application.Documents.Queries.GetDocumentById;

public record GetDocumentByIdQuery(Guid DocumentId) : IRequest<DocumentDetailDto>;