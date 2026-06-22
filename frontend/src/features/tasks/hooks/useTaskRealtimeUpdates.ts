import { useQueryClient } from '@tanstack/react-query';
import { useSignalREvent } from '@/signalr/hooks/useSignalREvent';

interface TaskMovedPayload {
    taskId: string;
    projectId: string;
    newStateId: string;
    newOrderIndex: number;
}

interface TaskDto {
    id: string;
    stateId: string;
    orderIndex: number;
    title: string;
}

export const useTaskRealtimeUpdates = () => {
    const queryClient = useQueryClient();

    useSignalREvent<TaskMovedPayload>('TaskMoved', (data) => {
        queryClient.setQueryData<TaskDto[]>(['tasks', data.projectId], (oldData) => {
            if (!oldData) return [];

            return oldData.map((task) =>
                task.id === data.taskId
                    ? { ...task, stateId: data.newStateId, orderIndex: data.newOrderIndex }
                    : task
            );
        });
    });
};