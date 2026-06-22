import { useMutation, useQueryClient } from '@tanstack/react-query';
import { workspaceService } from '../services/workspace.service';
import { WORKSPACE_QUERY_KEYS } from '../constants/workspace.constants';
import { UpdateMemberRoleRequest } from '../types/workspace.types';

export const useUpdateMemberRoleMutation = (workspaceId: string) => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: ({ memberId, data }: { memberId: string; data: UpdateMemberRoleRequest }) =>
            workspaceService.updateMemberRole(workspaceId, memberId, data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: WORKSPACE_QUERY_KEYS.members(workspaceId) });
        },
    });
};