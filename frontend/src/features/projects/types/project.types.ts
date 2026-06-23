export enum ProjectRole {
    Owner = 'Owner',
    Admin = 'Admin',
    Member = 'Member',
    Viewer = 'Viewer',
}

export interface ProjectDto {
    id: string;
    workspaceId: string;
    name: string;
    description: string | null;
    color: string;
    isArchived: boolean;
    createdAt: string;
}

export interface ProjectMemberDto {
    id: string;
    projectId: string;
    userId: string;
    firstName: string;
    lastName: string;
    email: string;
    role: ProjectRole;
    joinedAt: string;
}

export interface CreateProjectRequest {
    workspaceId: string;
    name: string;
    description?: string;
    color?: string;
}

export interface UpdateProjectRequest {
    name: string;
    description?: string;
    color?: string;
    isArchived?: boolean;
}