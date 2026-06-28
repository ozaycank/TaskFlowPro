export enum PriorityLevel {
    Low = 0,
    Medium = 1,
    High = 2,
    Urgent = 3,
    Critical = 4
}

export interface TaskDto {
    id: string;
    title: string;
    description: string | null;
    stateId: string;
    priority: PriorityLevel;
    assigneeId: string | null;
    orderIndex: number;
    createdAt: string;
    sprintId: string | null; // null means it's in the backlog
}

export interface CreateTaskCommand {
    workspaceId: string;
    projectId: string;
    title: string;
    description?: string;
    priority: PriorityLevel;
    stateId: string;
    orderIndex: number;
}

export interface TransitionTaskStateCommand {
    taskId: string;
    newStateId: string;
    newOrderIndex: number;
}