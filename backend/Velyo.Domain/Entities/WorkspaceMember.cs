using Velyo.Domain.Common.Models;
using Velyo.Domain.Enums;

namespace Velyo.Domain.Entities;

public class WorkspaceMember : AuditableEntity
{
    public Guid WorkspaceId { get; private set; }
    public Guid UserId { get; private set; }
    public WorkspaceRole Role { get; private set; }

    protected WorkspaceMember() { }

    private WorkspaceMember(Guid workspaceId, Guid userId, WorkspaceRole role)
    {
        Id = Guid.NewGuid();
        WorkspaceId = workspaceId;
        UserId = userId;
        Role = role;
    }

    public static WorkspaceMember Create(Guid workspaceId, Guid userId, WorkspaceRole role)
    {
        return new WorkspaceMember(workspaceId, userId, role);
    }

    public void ChangeRole(WorkspaceRole newRole)
    {
        Role = newRole;
    }
}