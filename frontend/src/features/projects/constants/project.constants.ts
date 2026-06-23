export const PROJECT_QUERY_KEYS = {
    all: ['projects'] as const,
    list: (workspaceId: string) => ['projects', 'workspace', workspaceId] as const,
    detail: (projectId: string) => ['projects', 'detail', projectId] as const,
    members: (projectId: string) => ['projects', 'members', projectId] as const,
    workflow: (projectId: string) => ['projects', 'workflow', projectId] as const,
    sprints: (projectId: string) => ['projects', 'sprints', projectId] as const,
};