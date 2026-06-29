import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { workspaceSettingsApi, UpdateWorkspaceCommand } from '../api/workspace-settings.api';

export const WORKSPACE_SETTINGS_KEYS = {
    detail: (workspaceId: string) => ['workspace', 'detail', workspaceId] as const,
    lists: ['workspaces'] as const, // Ana workspace listesini de güncellemek için
};

export const useWorkspaceDetailQuery = (workspaceId: string | undefined) => {
    return useQuery({
        queryKey: WORKSPACE_SETTINGS_KEYS.detail(workspaceId!),
        queryFn: () => workspaceSettingsApi.getWorkspaceById(workspaceId!),
        enabled: !!workspaceId,
    });
};

export const useUpdateWorkspaceMutation = (workspaceId: string) => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (command: UpdateWorkspaceCommand) => workspaceSettingsApi.updateWorkspace(command),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: WORKSPACE_SETTINGS_KEYS.detail(workspaceId) });
            queryClient.invalidateQueries({ queryKey: WORKSPACE_SETTINGS_KEYS.lists });
        },
    });
};

export const useDeleteWorkspaceMutation = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (workspaceId: string) => workspaceSettingsApi.deleteWorkspace(workspaceId),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: WORKSPACE_SETTINGS_KEYS.lists });
            // Silinen workspace'in detay cache'ini temizle
            queryClient.clear();
        },
    });
};