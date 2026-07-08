using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Documents.Commands.DeleteDocument;

public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDocumentCommandHandler(IDocumentRepository documentRepository, IWorkspaceAuthorizationService authService, IUnitOfWork unitOfWork)
    {
        _documentRepository = documentRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = await _documentRepository.GetByIdAsync(request.DocumentId, cancellationToken);
        if (document == null) throw new NotFoundException(nameof(Document), request.DocumentId);

        await _authService.AuthorizeMembershipAsync(document.WorkspaceId, cancellationToken);

        var hasChildren = await _documentRepository.HasChildrenAsync(document.Id, cancellationToken);
        if (hasChildren) throw new InvalidOperationException("Cannot delete a document that has child documents.");

        _documentRepository.Delete(document);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}