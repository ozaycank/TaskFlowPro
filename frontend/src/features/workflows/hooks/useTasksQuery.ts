import { useQuery } from '@tanstack/react-query';
import { taskApi } from '../api/task.api';

export const TASK_QUERY_KEYS = {
    byProject: (projectId: string) => ['tasks', 'project', projectId] as const,
};

export const useTasksQuery = (projectId: string | undefined) => {
    return useQuery({
        queryKey: TASK_QUERY_KEYS.byProject(projectId!),
        queryFn: () => taskApi.getTasksByProject(projectId!),
        enabled: !!projectId,
    });
};