using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;
using Velyo.Domain.Enums;
using Velyo.Domain.Events; // WorkspaceInvitationCreatedEvent için eklendi

namespace Velyo.Application.Workspaces.Commands.InviteMember;

// FIXED: IRequestHandler<InviteMemberCommand, string> yerine sadece IRequestHandler<InviteMemberCommand>
public class InviteMemberCommandHandler : IRequestHandler<InviteMemberCommand>
{
    private readonly IWorkspaceInvitationRepository _invitationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IWorkspaceMemberRepository _memberRepository;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IWorkspaceRepository _workspaceRepository; // EKLENDİ
    private readonly IUnitOfWork _unitOfWork;

    public InviteMemberCommandHandler(
        IWorkspaceInvitationRepository invitationRepository,
        IUserRepository userRepository,
        IWorkspaceMemberRepository memberRepository,
        IWorkspaceAuthorizationService authService,
        IWorkspaceRepository workspaceRepository, // EKLENDİ
        IUnitOfWork unitOfWork)
    {
        _invitationRepository = invitationRepository;
        _userRepository = userRepository;
        _memberRepository = memberRepository;
        _authService = authService;
        _workspaceRepository = workspaceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(InviteMemberCommand request, CancellationToken cancellationToken)
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

        // Davet e-postasında çalışma alanı adını gösterebilmek için veritabanından Workspace'i çekiyoruz
        var workspace = await _workspaceRepository.GetByIdAsync(request.WorkspaceId, cancellationToken);
        if (workspace == null) throw new NotFoundException(nameof(Workspace), request.WorkspaceId);

        var invitation = WorkspaceInvitation.Create(request.WorkspaceId, request.Email, request.Role);

        // YENİ: Outbox mekanizmasının tetiklenmesi için Domain Event ekleniyor
        invitation.AddDomainEvent(new WorkspaceInvitationCreatedEvent(invitation, workspace.Name));

        _invitationRepository.Add(invitation);

        // Bu işlem SaveChangesAsync çağrıldığında Outbox tablosuna yazılacak!
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}