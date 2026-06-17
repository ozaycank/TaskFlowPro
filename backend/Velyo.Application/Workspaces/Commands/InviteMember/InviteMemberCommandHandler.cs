using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;
using Velyo.Domain.Enums;

namespace Velyo.Application.Workspaces.Commands.InviteMember;

public class InviteMemberCommandHandler : IRequestHandler<InviteMemberCommand, string>
{
    private readonly IWorkspaceInvitationRepository _invitationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IWorkspaceMemberRepository _memberRepository;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public InviteMemberCommandHandler(
        IWorkspaceInvitationRepository invitationRepository,
        IUserRepository userRepository,
        IWorkspaceMemberRepository memberRepository,
        IWorkspaceAuthorizationService authService,
        IUnitOfWork unitOfWork)
    {
        _invitationRepository = invitationRepository;
        _userRepository = userRepository;
        _memberRepository = memberRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(InviteMemberCommand request, CancellationToken cancellationToken)
    {
        // SAAS SECURITY: Only Admins or Owners can invite members
        await _authService.AuthorizeRoleAsync(request.WorkspaceId, WorkspaceRole.Admin, cancellationToken);

        // SAAS SECURITY: Prevent Admins from inviting someone as an Owner
        if (request.Role == WorkspaceRole.Owner)
        {
            await _authService.AuthorizeRoleAsync(request.WorkspaceId, WorkspaceRole.Owner, cancellationToken);
        }

        // Check if user is already a member
        var targetUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (targetUser != null)
        {
            var isAlreadyMember = await _memberRepository.IsUserMemberAsync(request.WorkspaceId, targetUser.Id, cancellationToken);
            if (isAlreadyMember) throw new InvalidOperationException("User is already a member of this workspace.");
        }

        // Prevent duplicate pending invitations
        var existingInvite = await _invitationRepository.GetPendingInvitationAsync(request.WorkspaceId, request.Email, cancellationToken);
        if (existingInvite != null) throw new InvalidOperationException("A pending invitation already exists for this email.");

        var invitation = WorkspaceInvitation.Create(request.WorkspaceId, request.Email, request.Role);

        _invitationRepository.Add(invitation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // In a real app, you would publish a DomainEvent here to trigger an email worker.
        // For now, we return the token to simulate the link generation.
        return invitation.Token;
    }
}