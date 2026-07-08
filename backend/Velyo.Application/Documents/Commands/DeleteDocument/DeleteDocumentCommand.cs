using MediatR;

namespace Velyo.Application.Documents.Commands.DeleteDocument;

public record DeleteDocumentCommand(Guid DocumentId) : IRequest;