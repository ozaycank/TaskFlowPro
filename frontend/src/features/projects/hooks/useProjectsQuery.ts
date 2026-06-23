import { useQuery } from '@tanstack/react-query';
import { projectService } from '../services/project.service';
import { PROJECT_QUERY_KEYS } from '../constants/project.constants';
import { useWorkspaceStore } from '@/features/workspace/stores/useWorkspaceStore';

export const useProjectsQuery = () => {
    const workspaceId = useWorkspaceStore((state) => state.activeWorkspaceId);

    return useQuery({
        queryKey: PROJECT_QUERY_KEYS.list(workspaceId!),
        queryFn: () => projectService.getProjectsByWorkspace(workspaceId!),
        enabled: !!workspaceId,
        staleTime: 1000 * 60 * 5,
    });
};