using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;
using Velyo.Domain.Enums;

namespace Velyo.Application.Workspaces.Commands.UpdateWorkspace;

public class UpdateWorkspaceCommandHandler : IRequestHandler<UpdateWorkspaceCommand>
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateWorkspaceCommandHandler(IWorkspaceRepository workspaceRepository, IWorkspaceAuthorizationService authService, IUnitOfWork unitOfWork)
    {
        _workspaceRepository = workspaceRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        // SECURE: Sadece Admin veya Owner güncelleyebilir
        await _authService.AuthorizeRoleAsync(request.WorkspaceId, WorkspaceRole.Admin, cancellationToken);

        var workspace = await _workspaceRepository.GetByIdAsync(request.WorkspaceId, cancellationToken);
        if (workspace == null) throw new NotFoundException(nameof(Workspace), request.WorkspaceId);

        workspace.UpdateDetails(request.Name, request.Description);

        _workspaceRepository.Update(workspace);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}