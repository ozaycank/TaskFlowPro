import { apiClient } from '@/api/client';
import { CreateWorkflowStateRequest, UpdateWorkflowStateRequest, WorkflowStateDto } from '../types/workflow.types';

export const workflowApi = {
    getWorkflowStates: async (workspaceId: string): Promise<WorkflowStateDto[]> => {
        const response = await apiClient.get<WorkflowStateDto[]>(`/workflows/workspaces/${workspaceId}/states`);
        return response.data;
    },

    createWorkflowState: async (request: CreateWorkflowStateRequest): Promise<{ id: string }> => {
        const { data } = await apiClient.post<{ id: string }>(`/workflows/${request.workflowId}/states`, request);
        return data;
    },

    updateWorkflowState: async (request: UpdateWorkflowStateRequest): Promise<void> => {
        await apiClient.put(`/workflows/${request.workflowId}/states/${request.stateId}`, request);
    },

    deleteWorkflowState: async (workflowId: string, stateId: string): Promise<void> => {
        await apiClient.delete(`/workflows/${workflowId}/states/${stateId}`);
    }
};