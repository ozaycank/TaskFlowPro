import { useMutation, useQueryClient } from '@tanstack/react-query';
import { projectService } from '../services/project.service';
import { PROJECT_QUERY_KEYS } from '../constants/project.constants';
import { useWorkspaceStore } from '@/features/workspace/stores/useWorkspaceStore';

export const useDeleteProjectMutation = () => {
    const queryClient = useQueryClient();
    const workspaceId = useWorkspaceStore((state) => state.activeWorkspaceId);

    return useMutation({
        mutationFn: (projectId: string) => projectService.deleteProject(projectId),
        onSuccess: () => {
            if (workspaceId) {
                queryClient.invalidateQueries({ queryKey: PROJECT_QUERY_KEYS.list(workspaceId) });
            }
        },
    });
};