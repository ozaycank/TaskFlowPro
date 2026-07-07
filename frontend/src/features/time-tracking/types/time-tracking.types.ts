export interface WorklogDto {
    id: string;
    taskId: string;
    userId: string;
    timeSpentSeconds: number;
    startDate: string;
    description: string | null;
    createdAt: string;
}

export interface CreateWorklogCommand {
    taskId: string;
    userId: string; // In a real app, backend infers this. Passing a dummy/active user ID if required.
    timeSpentSeconds: number;
    startDate: string;
    description?: string;
}

export interface UpdateWorklogCommand {
    worklogId: string;
    timeSpentSeconds: number;
    startDate: string;
    description?: string;
}