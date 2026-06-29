using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;
using Velyo.Domain.Enums;

namespace Velyo.Application.Workspaces.Commands.DeleteWorkspace;

public class DeleteWorkspaceCommandHandler : IRequestHandler<DeleteWorkspaceCommand>
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteWorkspaceCommandHandler(IWorkspaceRepository workspaceRepository, IWorkspaceAuthorizationService authService, IUnitOfWork unitOfWork)
    {
        _workspaceRepository = workspaceRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteWorkspaceCommand request, CancellationToken cancellationToken)
    {
        // SECURE: SADECE Owner silebilir
        await _authService.AuthorizeRoleAsync(request.WorkspaceId, WorkspaceRole.Owner, cancellationToken);

        var workspace = await _workspaceRepository.GetByIdAsync(request.WorkspaceId, cancellationToken);
        if (workspace == null) throw new NotFoundException(nameof(Workspace), request.WorkspaceId);

        _workspaceRepository.Delete(workspace);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}