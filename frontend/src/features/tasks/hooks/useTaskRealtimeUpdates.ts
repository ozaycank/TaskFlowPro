import { useQueryClient } from '@tanstack/react-query';
import { useSignalREvent } from '@/signalr/hooks/useSignalREvent';
import { TaskDto } from '../../workflows/types/task.types';
import { TASK_QUERY_KEYS } from '../../workflows/hooks/useTasksQuery';

interface TaskMovedPayload {
    taskId: string;
    projectId: string;
    newStateId: string;
    newOrderIndex: number;
}

export const useTaskRealtimeUpdates = () => {
    const queryClient = useQueryClient();

    useSignalREvent<TaskMovedPayload>('TaskMoved', (data) => {
        queryClient.setQueryData<TaskDto[]>(TASK_QUERY_KEYS.byProject(data.projectId), (oldData) => {
            if (!oldData) return [];

            return oldData.map((task) =>
                task.id === data.taskId
                    ? { ...task, stateId: data.newStateId, orderIndex: data.newOrderIndex }
                    : task
            ).sort((a, b) => a.orderIndex - b.orderIndex);
        });
    });
};