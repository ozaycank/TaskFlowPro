import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { collaborationApi } from '../api/collaboration.api';
import { CreateCommentCommand } from '../types/collaboration.types';

export const COLLAB_KEYS = {
    comments: (taskId: string) => ['tasks', 'comments', taskId] as const,
    attachments: (taskId: string) => ['tasks', 'attachments', taskId] as const,
};

// --- Comments ---
export const useCommentsQuery = (taskId: string | undefined) => {
    return useQuery({
        queryKey: COLLAB_KEYS.comments(taskId!),
        queryFn: () => collaborationApi.getComments(taskId!),
        enabled: !!taskId,
    });
};

export const useCreateCommentMutation = (taskId: string) => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (command: CreateCommentCommand) => collaborationApi.createComment(command),
        onSuccess: () => {
            // Invalidate cache immediately so UI updates
            queryClient.invalidateQueries({ queryKey: COLLAB_KEYS.comments(taskId) });
        },
    });
};

// --- Attachments ---
export const useAttachmentsQuery = (taskId: string | undefined) => {
    return useQuery({
        queryKey: COLLAB_KEYS.attachments(taskId!),
        queryFn: () => collaborationApi.getAttachments(taskId!),
        enabled: !!taskId,
    });
};