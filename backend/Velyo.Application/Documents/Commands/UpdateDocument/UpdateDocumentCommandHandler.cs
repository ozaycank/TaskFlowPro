using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Documents.Commands.UpdateDocument;

public class UpdateDocumentCommandHandler : IRequestHandler<UpdateDocumentCommand>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly ISearchProjectionRepository _searchProjectionRepository; // Injected for Search Indexing
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDocumentCommandHandler(
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

    public async Task Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = await _documentRepository.GetByIdAsync(request.DocumentId, cancellationToken);
        if (document == null) throw new NotFoundException(nameof(Document), request.DocumentId);

        await _authService.AuthorizeMembershipAsync(document.WorkspaceId, cancellationToken);

        document.UpdateContent(request.Title, request.Content, request.EmojiIcon);
        _documentRepository.Update(document);

        var projection = await _searchProjectionRepository.GetByEntityIdAsync(document.Id, cancellationToken);
        if (projection != null)
        {
            projection.UpdateContent(request.Title, request.Content, request.EmojiIcon);
            _searchProjectionRepository.Update(projection);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}