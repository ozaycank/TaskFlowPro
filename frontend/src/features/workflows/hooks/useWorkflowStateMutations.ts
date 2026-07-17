import { useMutation, useQueryClient } from '@tanstack/react-query';
import { workflowApi } from '../api/workflow.api';
import { CreateWorkflowStateRequest, UpdateWorkflowStateRequest } from '../types/workflow.types';

export const useCreateWorkflowStateMutation = (workspaceId: string) => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (data: CreateWorkflowStateRequest) => workflowApi.createWorkflowState(data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['workflow-states', workspaceId] });
        },
    });
};

export const useUpdateWorkflowStateMutation = (workspaceId: string) => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (data: UpdateWorkflowStateRequest) => workflowApi.updateWorkflowState(data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['workflow-states', workspaceId] });
        },
    });
};

export const useDeleteWorkflowStateMutation = (workspaceId: string) => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: ({ workflowId, stateId }: { workflowId: string; stateId: string }) =>
            workflowApi.deleteWorkflowState(workflowId, stateId),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['workflow-states', workspaceId] });
        },
    });
};