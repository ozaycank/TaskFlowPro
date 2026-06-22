import { useQuery } from '@tanstack/react-query';
import { workspaceService } from '../services/workspace.service';
import { WORKSPACE_QUERY_KEYS } from '../constants/workspace.constants';

export const useWorkspacesQuery = () => {
    return useQuery({
        queryKey: WORKSPACE_QUERY_KEYS.all,
        queryFn: () => workspaceService.getWorkspaces(),
        staleTime: 1000 * 60 * 10, // 10 minutes
    });
};