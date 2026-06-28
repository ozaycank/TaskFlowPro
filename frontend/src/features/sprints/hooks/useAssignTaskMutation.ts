import { useMutation, useQueryClient } from '@tanstack/react-query';
import { taskApi } from '@/features/tasks/api/task.api';
import { TASK_QUERY_KEYS } from '@/features/workflows/hooks/useTasksQuery';
import { TaskDto } from '@/features/workflows/types/task.types';

export const useAssignTaskMutation = (projectId: string) => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: ({ taskId, sprintId }: { taskId: string; sprintId: string | null }) =>
            taskApi.assignToSprint(taskId, sprintId),
        onMutate: async ({ taskId, sprintId }) => {
            const queryKey = TASK_QUERY_KEYS.byProject(projectId);
            await queryClient.cancelQueries({ queryKey });

            const previousTasks = queryClient.getQueryData<TaskDto[]>(queryKey);

            if (previousTasks) {
                queryClient.setQueryData<TaskDto[]>(queryKey, (old) =>
                    old?.map(task =>
                        task.id === taskId ? { ...task, sprintId } : task
                    )
                );
            }

            return { previousTasks };
        },
        onError: (err, newCommand, context) => {
            if (context?.previousTasks) {
                queryClient.setQueryData(TASK_QUERY_KEYS.byProject(projectId), context.previousTasks);
            }
        },
        onSettled: () => {
            queryClient.invalidateQueries({ queryKey: TASK_QUERY_KEYS.byProject(projectId) });
        }
    });
};