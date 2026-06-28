export enum SprintStatus {
    Planned = 10,
    Active = 20,
    Completed = 30,
    Closed = 40
}

export interface SprintDto {
    id: string;
    projectId: string;
    name: string;
    goal: string | null;
    startDate: string | null;
    endDate: string | null;
    status: SprintStatus;
    createdAt: string;
}

export interface CreateSprintCommand {
    projectId: string;
    name: string;
    goal?: string;
    startDate?: string | null;
    endDate?: string | null;
}

export interface UpdateSprintCommand {
    sprintId: string;
    name: string;
    goal?: string;
    startDate?: string | null;
    endDate?: string | null;
}

export interface AssignTaskToSprintCommand {
    taskId: string;
    sprintId: string | null; // null means send to backlog
}