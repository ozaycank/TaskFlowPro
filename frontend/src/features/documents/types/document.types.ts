export interface DocumentDto {
    id: string;
    workspaceId: string;
    projectId: string | null;
    parentDocumentId: string | null;
    title: string;
    emojiIcon: string | null;
    orderIndex: number;
    createdAt: string;
    lastModifiedAt: string | null;
}

export interface DocumentDetailDto extends DocumentDto {
    content: string;
}

export interface CreateDocumentRequest {
    workspaceId: string;
    projectId?: string | null;
    parentDocumentId?: string | null;
    title: string;
    content: string;
    emojiIcon?: string | null;
    orderIndex: number;
}

export interface UpdateDocumentRequest {
    documentId: string;
    title: string;
    content: string;
    emojiIcon?: string | null;
}