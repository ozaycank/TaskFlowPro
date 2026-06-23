import { apiClient } from '@/api/client';
import { WorkflowStateDto } from '../types/workflow.types';

export const workflowApi = {
    getWorkflowStates: async (workspaceId: string): Promise<WorkflowStateDto[]> => {
        const response = await apiClient.get<WorkflowStateDto[]>(`/workflows/workspaces/${workspaceId}/states`);
        return response.data;
    }
};