import { useMutation, useQueryClient } from '@tanstack/react-query';
import { workspaceService } from '../services/workspace.service';
import { WORKSPACE_QUERY_KEYS } from '../constants/workspace.constants';
import { InviteMemberRequest } from '../types/workspace.types';

export const useInviteMemberMutation = (workspaceId: string) => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (data: InviteMemberRequest) => workspaceService.inviteMember(workspaceId, data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: WORKSPACE_QUERY_KEYS.members(workspaceId) });
        },
    });
};