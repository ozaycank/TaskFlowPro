export enum PriorityLevel {
    Low = 1,     // Backend Domain ile eşleşecek şekilde 1'den başlatıldı
    Medium = 2,
    High = 3,
    Urgent = 4,
    Critical = 5
}

export interface TaskDto {
    id: string;
    title: string;
    description: string | null;
    stateId: string;
    priority: PriorityLevel;
    assigneeId: string | null;
    orderIndex: number;
    dueDate?: string | null; // Phase 26 addition
    createdAt: string;
    customFieldsData: Record<string, string>;
}

// Phase 26 Addition: Detailed DTO for the Task View
export interface TaskDetailDto extends TaskDto {
    projectId: string;
}

export interface CreateTaskCommand {
    workspaceId: string;
    projectId: string;
    title: string;
    description?: string;
    priority: PriorityLevel;
    stateId: string;
    orderIndex: number;
    dueDate?: string | null; // Phase 26 addition
}

export interface UpdateTaskCommand {
    taskId: string;
    title: string;
    description?: string;
    priority: PriorityLevel;
    dueDate?: string | null; // Phase 26 addition
}

export interface TransitionTaskStateCommand {
    taskId: string;
    newStateId: string;
    newOrderIndex: number;
}