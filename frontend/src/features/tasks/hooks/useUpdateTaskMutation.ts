import { useMutation, useQueryClient } from '@tanstack/react-query';
import { taskApi } from '../api/task.api';
import { UpdateTaskCommand } from '../types/task.types';
import { TASK_QUERY_KEYS } from '../../workflows/hooks/useTasksQuery';
import { TASK_DETAIL_QUERY_KEY } from './useTaskQuery';

export const useUpdateTaskMutation = (projectId: string, taskId: string) => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (command: UpdateTaskCommand) => taskApi.updateTask(command),
        onSuccess: () => {
            // Hem tahtayı hem de detay sayfasını yenile
            queryClient.invalidateQueries({ queryKey: TASK_QUERY_KEYS.byProject(projectId) });
            queryClient.invalidateQueries({ queryKey: TASK_DETAIL_QUERY_KEY(taskId) });
        },
    });
};