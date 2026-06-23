import { useQuery } from '@tanstack/react-query';
import { projectService } from '../services/project.service';
import { PROJECT_QUERY_KEYS } from '../constants/project.constants';

export const useProjectQuery = (projectId: string | null) => {
    return useQuery({
        queryKey: PROJECT_QUERY_KEYS.detail(projectId!),
        queryFn: () => projectService.getProjectById(projectId!),
        enabled: !!projectId,
    });
};