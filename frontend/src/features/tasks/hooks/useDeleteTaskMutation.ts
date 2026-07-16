import { useMutation, useQueryClient } from '@tanstack/react-query';
import { taskApi } from '../api/task.api';

export const useDeleteTaskMutation = (projectId: string) => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (taskId: string) => taskApi.deleteTask(taskId),
        onSuccess: () => {
            // Invalidate the tasks cache for the current project
            queryClient.invalidateQueries({ queryKey: ['tasks', projectId] });
        },
    });
};