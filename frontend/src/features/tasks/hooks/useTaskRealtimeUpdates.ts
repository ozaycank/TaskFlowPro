import { useQueryClient } from '@tanstack/react-query';
import { useSignalREvent } from '@/signalr/hooks/useSignalREvent';
import { TaskDto } from '../../workflows/types/task.types';
import { TASK_QUERY_KEYS } from '../../workflows/hooks/useTasksQuery';
import { COLLAB_KEYS } from './useCollaborationHooks'; // NEW

interface TaskMovedPayload {
    taskId: string;
    projectId: string;
    newStateId: string;
    newOrderIndex: number;
}

interface CommentAddedPayload {
    commentId: string;
    taskId: string;
    userId: string;
    content: string;
    createdAt: string;
}

export const useTaskRealtimeUpdates = () => {
    const queryClient = useQueryClient();

    // 1. Task Movement Sync
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

    // 2. Comment Sync
    useSignalREvent<CommentAddedPayload>('CommentAdded', (data) => {
        // Just invalidate to fetch fresh data and user details
        queryClient.invalidateQueries({ queryKey: COLLAB_KEYS.comments(data.taskId) });
    });

    // 3. Attachment Sync
    useSignalREvent<any>('AttachmentUploaded', (data) => {
        queryClient.invalidateQueries({ queryKey: COLLAB_KEYS.attachments(data.taskId) });
    });
};