import { useMutation, useQueryClient } from '@tanstack/react-query';
import { taskApi } from '../api/task.api';
import { CreateTaskCommand } from '../types/task.types';
import { TASK_QUERY_KEYS } from '../../workflows/hooks/useTasksQuery';

export const useCreateTaskMutation = (projectId: string) => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (command: CreateTaskCommand) => taskApi.createTask(command),
        onSuccess: () => {
            // Task listesini yenile (Kanban board anında güncellenir)
            queryClient.invalidateQueries({ queryKey: TASK_QUERY_KEYS.byProject(projectId) });
        },
    });
};