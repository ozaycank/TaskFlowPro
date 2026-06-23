import { useMutation, useQueryClient } from '@tanstack/react-query';
import { projectService } from '../services/project.service';
import { PROJECT_QUERY_KEYS } from '../constants/project.constants';
import { UpdateProjectRequest } from '../types/project.types';
import { useWorkspaceStore } from '@/features/workspace/stores/useWorkspaceStore';

export const useUpdateProjectMutation = (projectId: string) => {
    const queryClient = useQueryClient();
    const workspaceId = useWorkspaceStore((state) => state.activeWorkspaceId);

    return useMutation({
        mutationFn: (data: UpdateProjectRequest) => projectService.updateProject(projectId, data),
        onSuccess: (updatedProject) => {
            queryClient.setQueryData(PROJECT_QUERY_KEYS.detail(projectId), updatedProject);
            if (workspaceId) {
                queryClient.invalidateQueries({ queryKey: PROJECT_QUERY_KEYS.list(workspaceId) });
            }
        },
    });
};