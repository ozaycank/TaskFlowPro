import { apiClient } from '@/api/client';
import {
    CommentDto,
    AttachmentDto,
    CreateCommentCommand,
    RequestUploadUrlCommand,
    UploadUrlResponseDto,
    CompleteUploadCommand
} from '../types/collaboration.types';

export const collaborationApi = {
    // --- Comments ---
    getComments: async (taskId: string): Promise<CommentDto[]> => {
        const response = await apiClient.get<CommentDto[]>(`/tasks/${taskId}/comments`);
        return response.data;
    },
    createComment: async (command: CreateCommentCommand): Promise<string> => {
        const response = await apiClient.post<string>(`/tasks/${command.taskId}/comments`, command);
        return response.data;
    },

    // --- Attachments ---
    getAttachments: async (taskId: string): Promise<AttachmentDto[]> => {
        const response = await apiClient.get<AttachmentDto[]>(`/tasks/${taskId}/attachments`);
        return response.data;
    },
    requestUploadUrl: async (command: RequestUploadUrlCommand): Promise<UploadUrlResponseDto> => {
        const response = await apiClient.post<UploadUrlResponseDto>(`/tasks/${command.taskId}/attachments/request-upload`, command);
        return response.data;
    },
    completeUpload: async (taskId: string, command: CompleteUploadCommand): Promise<void> => {
        await apiClient.post(`/tasks/${taskId}/attachments/complete-upload`, command);
    }
};