export interface ActivityLogDto {
    id: string;
    workspaceId: string;
    projectId: string | null;
    taskId: string | null;
    userId: string;
    entityType: string;
    entityId: string;
    action: string;
    details: string | null;
    createdAt: string;
}

export interface ActivityQueryParams {
    workspaceId?: string;
    projectId?: string;
    taskId?: string;
    userId?: string;
    limit?: number;
}