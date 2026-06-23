import { useMutation, useQueryClient } from '@tanstack/react-query';
import { projectService } from '../services/project.service';
import { PROJECT_QUERY_KEYS } from '../constants/project.constants';
import { CreateProjectRequest } from '../types/project.types';
import { useWorkspaceStore } from '@/features/workspace/stores/useWorkspaceStore';

export const useCreateProjectMutation = () => {
    const queryClient = useQueryClient();
    const workspaceId = useWorkspaceStore((state) => state.activeWorkspaceId);

    return useMutation({
        mutationFn: (data: CreateProjectRequest) => projectService.createProject(data),
        onSuccess: () => {
            if (workspaceId) {
                queryClient.invalidateQueries({ queryKey: PROJECT_QUERY_KEYS.list(workspaceId) });
            }
        },
    });
};