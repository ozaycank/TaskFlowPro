import { apiClient } from '@/api/client';

export interface WorkspaceDetailDto {
    id: string;
    name: string;
    description: string | null;
    ownerId: string;
    createdAt: string;
}

export interface UpdateWorkspaceCommand {
    workspaceId: string;
    name: string;
    description?: string;
}

export const workspaceSettingsApi = {
    getWorkspaceById: async (workspaceId: string): Promise<WorkspaceDetailDto> => {
        const { data } = await apiClient.get<WorkspaceDetailDto>(`/workspaces/${workspaceId}`);
        return data;
    },

    updateWorkspace: async (command: UpdateWorkspaceCommand): Promise<void> => {
        await apiClient.put(`/workspaces/${command.workspaceId}`, command);
    },

    deleteWorkspace: async (workspaceId: string): Promise<void> => {
        await apiClient.delete(`/workspaces/${workspaceId}`);
    }
};