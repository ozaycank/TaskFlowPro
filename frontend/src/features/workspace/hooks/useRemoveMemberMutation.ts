import { useMutation, useQueryClient } from '@tanstack/react-query';
import { workspaceService } from '../services/workspace.service';
import { WORKSPACE_QUERY_KEYS } from '../constants/workspace.constants';

export const useRemoveMemberMutation = (workspaceId: string) => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (memberId: string) => workspaceService.removeMember(workspaceId, memberId),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: WORKSPACE_QUERY_KEYS.members(workspaceId) });
        },
    });
};