import { useQuery } from '@tanstack/react-query';
import { taskApi } from '../api/task.api';

export const TASK_DETAIL_QUERY_KEY = (taskId: string) => ['tasks', 'detail', taskId] as const;

export const useTaskQuery = (taskId: string | undefined) => {
    return useQuery({
        queryKey: TASK_DETAIL_QUERY_KEY(taskId!),
        queryFn: () => taskApi.getTaskById(taskId!),
        enabled: !!taskId,
    });
};