import { useQuery } from '@tanstack/react-query';
import { workspaceService } from '../services/workspace.service';
import { WORKSPACE_QUERY_KEYS } from '../constants/workspace.constants';

export const useWorkspaceMembersQuery = (workspaceId: string | null) => {
    return useQuery({
        queryKey: WORKSPACE_QUERY_KEYS.members(workspaceId!),
        queryFn: () => workspaceService.getWorkspaceMembers(workspaceId!),
        enabled: !!workspaceId,
    });
};