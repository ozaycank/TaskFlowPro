export enum WorkspaceRole {
    Owner = 'Owner',
    Admin = 'Admin',
    Member = 'Member',
    Guest = 'Guest',
}

export interface WorkspaceDto {
    id: string;
    name: string;
    description: string | null;
    ownerId: string;
    subscriptionPlanId: string;
    subscriptionStatus: string;
    createdAt: string;
}

export interface WorkspaceMemberDto {
    id: string;
    workspaceId: string;
    userId: string;
    email: string;
    firstName: string;
    lastName: string;
    role: WorkspaceRole;
    joinedAt: string;
}

export interface CreateWorkspaceRequest {
    name: string;
    description?: string;
}

export interface InviteMemberRequest {
    email: string;
    role: WorkspaceRole;
}

export interface UpdateMemberRoleRequest {
    role: WorkspaceRole;
}