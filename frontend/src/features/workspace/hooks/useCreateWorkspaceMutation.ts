import { useMutation, useQueryClient } from '@tanstack/react-query';
import { workspaceService } from '../services/workspace.service';
import { WORKSPACE_QUERY_KEYS } from '../constants/workspace.constants';
import { CreateWorkspaceRequest } from '../types/workspace.types';

export const useCreateWorkspaceMutation = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (data: CreateWorkspaceRequest) => workspaceService.createWorkspace(data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: WORKSPACE_QUERY_KEYS.all });
        },
    });
};