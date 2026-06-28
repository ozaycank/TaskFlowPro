export enum NotificationType {
    System = 10,
    WorkspaceInvitation = 20,
    RoleChanged = 30,
    ProjectCreated = 40,
    TaskAssigned = 50,
    TaskStatusChanged = 60,
    Mentioned = 70
}

export interface NotificationDto {
    id: string;
    workspaceId: string;
    type: NotificationType;
    title: string;
    message: string;
    actionUrl: string | null;
    isRead: boolean;
    createdAt: string;
}