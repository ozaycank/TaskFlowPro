import { useQuery } from '@tanstack/react-query';
import { workspaceService } from '../services/workspace.service';
import { WORKSPACE_QUERY_KEYS } from '../constants/workspace.constants';

export const useWorkspaceQuery = (workspaceId: string | null) => {
    return useQuery({
        queryKey: WORKSPACE_QUERY_KEYS.detail(workspaceId!),
        queryFn: () => workspaceService.getWorkspaceById(workspaceId!),
        enabled: !!workspaceId,
    });
};