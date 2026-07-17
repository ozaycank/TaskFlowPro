export const DOCUMENT_QUERY_KEYS = {
    all: ['documents'] as const,
    tree: (workspaceId: string, projectId?: string | null) =>
        [...DOCUMENT_QUERY_KEYS.all, 'tree', workspaceId, projectId ?? 'workspace'] as const,
    detail: (documentId: string) =>
        [...DOCUMENT_QUERY_KEYS.all, 'detail', documentId] as const,
};