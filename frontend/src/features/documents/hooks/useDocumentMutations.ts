import { useMutation, useQueryClient } from '@tanstack/react-query';
import { documentApi } from '../api/document.api';
import { DOCUMENT_QUERY_KEYS } from '../constants/document.constants';
import { CreateDocumentRequest, UpdateDocumentRequest } from '../types/document.types';

export const useCreateDocumentMutation = (workspaceId: string, projectId?: string | null) => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (data: CreateDocumentRequest) => documentApi.createDocument(data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: DOCUMENT_QUERY_KEYS.tree(workspaceId, projectId) });
        },
    });
};

export const useUpdateDocumentMutation = (workspaceId: string, projectId?: string | null) => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (data: UpdateDocumentRequest) => documentApi.updateDocument(data),
        onSuccess: (_, variables) => {
            queryClient.invalidateQueries({ queryKey: DOCUMENT_QUERY_KEYS.tree(workspaceId, projectId) });
            queryClient.invalidateQueries({ queryKey: DOCUMENT_QUERY_KEYS.detail(variables.documentId) });
        },
    });
};

export const useDeleteDocumentMutation = (workspaceId: string, projectId?: string | null) => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (documentId: string) => documentApi.deleteDocument(documentId),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: DOCUMENT_QUERY_KEYS.tree(workspaceId, projectId) });
        },
    });
};