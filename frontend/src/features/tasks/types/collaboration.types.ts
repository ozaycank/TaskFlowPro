export interface CommentDto {
    id: string;
    taskId: string;
    userId: string;
    content: string;
    isEdited: boolean;
    createdAt: string;
    createdBy: string | null;
}

export interface AttachmentDto {
    id: string;
    taskId: string;
    fileName: string;
    contentType: string;
    fileSizeBytes: number;
    isUploaded: boolean;
    createdAt: string;
}

export interface CreateCommentCommand {
    taskId: string;
    content: string;
}

export interface RequestUploadUrlCommand {
    taskId: string;
    commentId?: string;
    fileName: string;
    contentType: string;
    fileSizeBytes: number;
}

export interface UploadUrlResponseDto {
    attachmentId: string;
    presignedUrl: string;
}

export interface CompleteUploadCommand {
    attachmentId: string;
}