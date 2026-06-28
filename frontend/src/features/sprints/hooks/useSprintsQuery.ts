import { useQuery } from '@tanstack/react-query';
import { sprintApi } from '../api/sprint.api';

export const SPRINT_QUERY_KEYS = {
    byProject: (projectId: string) => ['sprints', 'project', projectId] as const,
};

export const useSprintsQuery = (projectId: string | undefined) => {
    return useQuery({
        queryKey: SPRINT_QUERY_KEYS.byProject(projectId!),
        queryFn: () => sprintApi.getSprintsByProject(projectId!),
        enabled: !!projectId,
    });
};