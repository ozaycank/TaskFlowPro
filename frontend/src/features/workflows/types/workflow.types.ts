// Assuming Enums matching the backend
export enum StateCategory {
    ToDo = 0,
    InProgress = 1,
    Done = 2
}

export interface WorkflowStateDto {
    id: string;
    name: string;
    color: string;
    category: StateCategory; // Added Category to match backend if needed
    orderIndex: number;
}

export interface CreateWorkflowStateRequest {
    workflowId: string;
    name: string;
    color: string;
    category: StateCategory;
    orderIndex: number;
}

export interface UpdateWorkflowStateRequest {
    workflowId: string;
    stateId: string;
    name: string;
    color: string;
    category: StateCategory;
    orderIndex: number;
}