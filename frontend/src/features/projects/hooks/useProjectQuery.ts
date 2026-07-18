import { useQuery } from '@tanstack/react-query';
import { projectService } from '../services/project.service';
import { PROJECT_QUERY_KEYS } from '../constants/project.constants';
import { useWorkspaceStore } from '@/features/workspace/stores/useWorkspaceStore';

export const useProjectQuery = (projectId: string | null) => {
    const workspaceId = useWorkspaceStore((state) => state.activeWorkspaceId);

    return useQuery({
        queryKey: [...PROJECT_QUERY_KEYS.detail(projectId!), workspaceId],
        queryFn: () => projectService.getProjectById(projectId!, workspaceId!), // Servise parametre olarak geçiyoruz
        enabled: !!projectId && !!workspaceId,
    });
};