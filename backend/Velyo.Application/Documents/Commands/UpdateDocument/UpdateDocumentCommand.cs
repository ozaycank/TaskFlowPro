using MediatR;

namespace Velyo.Application.Documents.Commands.UpdateDocument;

public record UpdateDocumentCommand(
    Guid DocumentId,
    string Title,
    string Content,
    string? EmojiIcon) : IRequest;