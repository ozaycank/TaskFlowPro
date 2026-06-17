using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Enums;

namespace Velyo.Application.Workspaces.Commands.RemoveMember;

public record RemoveMemberCommand(Guid WorkspaceId, Guid UserIdToRemove) : IRequest;

public class RemoveMemberCommandHandler : IRequestHandler<RemoveMemberCommand>
{
    private readonly IWorkspaceMemberRepository _memberRepository;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveMemberCommandHandler(
        IWorkspaceMemberRepository memberRepository,
        IWorkspaceAuthorizationService authService,
        IUnitOfWork unitOfWork)
    {
        _memberRepository = memberRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveMemberCommand request, CancellationToken cancellationToken)
    {
        // SAAS SECURITY: Only Admins or Owners can remove members
        await _authService.AuthorizeRoleAsync(request.WorkspaceId, WorkspaceRole.Admin, cancellationToken);

        var targetMember = await _memberRepository.GetMemberAsync(request.WorkspaceId, request.UserIdToRemove, cancellationToken);
        if (targetMember == null) throw new NotFoundException("WorkspaceMember", request.UserIdToRemove);

        // SAAS SECURITY: Admins cannot remove Owners
        if (targetMember.Role == WorkspaceRole.Owner)
        {
            throw new InvalidOperationException("The workspace owner cannot be removed.");
        }

        _memberRepository.Remove(targetMember); // Requires Remove method in interface
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}