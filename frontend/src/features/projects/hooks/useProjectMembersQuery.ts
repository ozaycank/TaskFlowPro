import { useQuery } from '@tanstack/react-query';
import { projectService } from '../services/project.service';
import { PROJECT_QUERY_KEYS } from '../constants/project.constants';

export const useProjectMembersQuery = (projectId: string | null) => {
    return useQuery({
        queryKey: PROJECT_QUERY_KEYS.members(projectId!),
        queryFn: () => projectService.getProjectMembers(projectId!),
        enabled: !!projectId,
    });
};