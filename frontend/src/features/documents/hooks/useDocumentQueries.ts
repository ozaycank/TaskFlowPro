import { useQuery } from '@tanstack/react-query';
import { documentApi } from '../api/document.api';
import { DOCUMENT_QUERY_KEYS } from '../constants/document.constants';

export const useDocumentTreeQuery = (workspaceId: string, projectId?: string | null) => {
    return useQuery({
        queryKey: DOCUMENT_QUERY_KEYS.tree(workspaceId, projectId),
        queryFn: () => documentApi.getDocumentTree(workspaceId, projectId),
        enabled: !!workspaceId,
    });
};

export const useDocumentDetailQuery = (documentId: string) => {
    return useQuery({
        queryKey: DOCUMENT_QUERY_KEYS.detail(documentId),
        queryFn: () => documentApi.getDocumentById(documentId),
        enabled: !!documentId,
    });
};