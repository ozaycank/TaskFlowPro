using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Workspaces.Commands.AcceptInvitation;

public class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand>
{
    private readonly IWorkspaceInvitationRepository _invitationRepository;
    private readonly IWorkspaceMemberRepository _memberRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public AcceptInvitationCommandHandler(
        IWorkspaceInvitationRepository invitationRepository,
        IWorkspaceMemberRepository memberRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _invitationRepository = invitationRepository;
        _memberRepository = memberRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
    {
        var invitation = await _invitationRepository.GetByTokenAsync(request.Token, cancellationToken);
        if (invitation == null) throw new NotFoundException(nameof(WorkspaceInvitation), "Token");

        // SAAS SECURITY: Ensure the logged-in user matches the invited email
        if (!string.Equals(_currentUserService.Email, invitation.Email, StringComparison.OrdinalIgnoreCase))
        {
            throw new ForbiddenAccessException("This invitation was sent to a different email address.");
        }

        var userId = Guid.Parse(_currentUserService.UserId!);

        // Check if already a member somehow
        var isAlreadyMember = await _memberRepository.IsUserMemberAsync(invitation.WorkspaceId, userId, cancellationToken);
        if (isAlreadyMember) throw new InvalidOperationException("You are already a member of this workspace.");

        invitation.Accept();

        var newMember = WorkspaceMember.Create(invitation.WorkspaceId, userId, invitation.Role);
        _memberRepository.Add(newMember);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}