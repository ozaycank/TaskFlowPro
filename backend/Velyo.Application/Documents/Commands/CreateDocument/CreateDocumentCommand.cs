using MediatR;

namespace Velyo.Application.Documents.Commands.CreateDocument;

public record CreateDocumentCommand(
    Guid WorkspaceId,
    Guid? ProjectId,
    Guid? ParentDocumentId,
    string Title,
    string Content,
    string? EmojiIcon,
    float OrderIndex) : IRequest<Guid>;