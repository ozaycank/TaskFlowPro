using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Documents.Queries.GetDocumentById;

public class GetDocumentByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, DocumentDetailDto>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IWorkspaceAuthorizationService _authService;

    public GetDocumentByIdQueryHandler(IDocumentRepository documentRepository, IWorkspaceAuthorizationService authService)
    {
        _documentRepository = documentRepository;
        _authService = authService;
    }

    public async Task<DocumentDetailDto> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
    {
        var document = await _documentRepository.GetByIdAsync(request.DocumentId, cancellationToken);
        if (document == null) throw new NotFoundException(nameof(Document), request.DocumentId);

        await _authService.AuthorizeMembershipAsync(document.WorkspaceId, cancellationToken);

        return new DocumentDetailDto(
            document.Id, document.WorkspaceId, document.ProjectId, document.ParentDocumentId,
            document.Title, document.Content, document.EmojiIcon, document.OrderIndex,
            document.CreatedAt, document.LastModifiedAt);
    }
}