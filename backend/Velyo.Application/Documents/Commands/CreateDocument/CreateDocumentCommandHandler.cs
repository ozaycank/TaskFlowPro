using MediatR;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Documents.Commands.CreateDocument;

public class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommand, Guid>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly ISearchProjectionRepository _searchProjectionRepository; // Injected for Search Indexing
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDocumentCommandHandler(
        IDocumentRepository documentRepository,
        ISearchProjectionRepository searchProjectionRepository,
        IWorkspaceAuthorizationService authService,
        IUnitOfWork unitOfWork)
    {
        _documentRepository = documentRepository;
        _searchProjectionRepository = searchProjectionRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
    {
        await _authService.AuthorizeMembershipAsync(request.WorkspaceId, cancellationToken);

        var document = Document.Create(request.WorkspaceId, request.ProjectId, request.ParentDocumentId, request.Title, request.Content, request.EmojiIcon, request.OrderIndex);

        _documentRepository.Add(document);

        var searchProjection = SearchProjection.CreateDocumentProjection(document);
        _searchProjectionRepository.Add(searchProjection);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return document.Id;
    }
}