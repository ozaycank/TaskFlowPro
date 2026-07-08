using MediatR;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;

namespace Velyo.Application.Documents.Queries.GetDocumentTree;

public class GetDocumentTreeQueryHandler : IRequestHandler<GetDocumentTreeQuery, IEnumerable<DocumentDto>>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IWorkspaceAuthorizationService _authService;

    public GetDocumentTreeQueryHandler(IDocumentRepository documentRepository, IWorkspaceAuthorizationService authService)
    {
        _documentRepository = documentRepository;
        _authService = authService;
    }

    public async Task<IEnumerable<DocumentDto>> Handle(GetDocumentTreeQuery request, CancellationToken cancellationToken)
    {
        await _authService.AuthorizeMembershipAsync(request.WorkspaceId, cancellationToken);

        var documents = request.ProjectId.HasValue
            ? await _documentRepository.GetByProjectIdAsync(request.ProjectId.Value, cancellationToken)
            : await _documentRepository.GetByWorkspaceIdAsync(request.WorkspaceId, cancellationToken);

        return documents.Select(d => new DocumentDto(
            d.Id, d.WorkspaceId, d.ProjectId, d.ParentDocumentId, d.Title, d.EmojiIcon, d.OrderIndex, d.CreatedAt, d.LastModifiedAt))
            .OrderBy(d => d.OrderIndex);
    }
}