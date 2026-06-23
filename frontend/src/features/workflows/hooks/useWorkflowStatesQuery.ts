import { useQuery } from '@tanstack/react-query';
import { workflowApi } from '../api/workflow.api';

export const WORKFLOW_QUERY_KEYS = {
    states: (workspaceId: string) => ['workflows', 'states', workspaceId] as const,
};

export const useWorkflowStatesQuery = (workspaceId: string | undefined) => {
    return useQuery({
        queryKey: WORKFLOW_QUERY_KEYS.states(workspaceId!),
        queryFn: () => workflowApi.getWorkflowStates(workspaceId!),
        enabled: !!workspaceId,
        staleTime: 1000 * 60 * 30, // 30 minutes (Workflow states rarely change)
    });
};