import { apiClient } from '@/api/client';
import {
    DocumentDto,
    DocumentDetailDto,
    CreateDocumentRequest,
    UpdateDocumentRequest
} from '../types/document.types';

export const documentApi = {
    getDocumentTree: async (workspaceId: string, projectId?: string | null): Promise<DocumentDto[]> => {
        const params = new URLSearchParams();
        if (projectId) {
            params.append('projectId', projectId);
        }

        const queryString = params.toString();
        const url = `/documents/workspaces/${workspaceId}${queryString ? `?${queryString}` : ''}`;

        const { data } = await apiClient.get<DocumentDto[]>(url);
        return data;
    },

    getDocumentById: async (documentId: string): Promise<DocumentDetailDto> => {
        const { data } = await apiClient.get<DocumentDetailDto>(`/documents/${documentId}`);
        return data;
    },

    createDocument: async (request: CreateDocumentRequest): Promise<string> => {
        const { data } = await apiClient.post<string>('/documents', request);
        return data;
    },

    updateDocument: async (request: UpdateDocumentRequest): Promise<void> => {
        await apiClient.put(`/documents/${request.documentId}`, request);
    },

    deleteDocument: async (documentId: string): Promise<void> => {
        await apiClient.delete(`/documents/${documentId}`);
    }
};