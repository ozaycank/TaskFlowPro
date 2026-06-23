import { useMutation, useQueryClient } from '@tanstack/react-query';
import { taskApi } from '../api/task.api';
import { TransitionTaskStateCommand, TaskDto } from '../types/task.types';
import { TASK_QUERY_KEYS } from './useTasksQuery';

export const useTransitionTaskMutation = (projectId: string) => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (command: TransitionTaskStateCommand) => taskApi.transitionTask(command),
        onMutate: async (newCommand) => {
            const queryKey = TASK_QUERY_KEYS.byProject(projectId);

            // Cancel any outgoing refetches to avoid overwriting optimistic update
            await queryClient.cancelQueries({ queryKey });

            // Snapshot previous value
            const previousTasks = queryClient.getQueryData<TaskDto[]>(queryKey);

            // Optimistically update the cache
            if (previousTasks) {
                queryClient.setQueryData<TaskDto[]>(queryKey, (old) =>
                    old?.map(task =>
                        task.id === newCommand.taskId
                            ? { ...task, stateId: newCommand.newStateId, orderIndex: newCommand.newOrderIndex }
                            : task
                    )
                );
            }

            return { previousTasks };
        },
        onError: (err, newCommand, context) => {
            // Rollback on error
            if (context?.previousTasks) {
                queryClient.setQueryData(TASK_QUERY_KEYS.byProject(projectId), context.previousTasks);
            }
        },
        onSettled: () => {
            // Sync with backend to ensure perfect state
            queryClient.invalidateQueries({ queryKey: TASK_QUERY_KEYS.byProject(projectId) });
        },
    });
};