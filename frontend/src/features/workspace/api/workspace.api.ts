import { apiClient } from '@/api/client';
import {
    CreateWorkspaceRequest,
    InviteMemberRequest,
    UpdateMemberRoleRequest,
    WorkspaceDto,
    WorkspaceMemberDto
} from '../types/workspace.types';

export const workspaceApi = {
    getWorkspaces: async (): Promise<WorkspaceDto[]> => {
        const { data } = await apiClient.get<WorkspaceDto[]>('/workspaces');
        return data;
    },

    getWorkspaceById: async (id: string): Promise<WorkspaceDto> => {
        const { data } = await apiClient.get<WorkspaceDto>(`/workspaces/${id}`);
        return data;
    },

    createWorkspace: async (request: CreateWorkspaceRequest): Promise<WorkspaceDto> => {
        const { data } = await apiClient.post<WorkspaceDto>('/workspaces', request);
        return data;
    },

    updateWorkspace: async (id: string, request: CreateWorkspaceRequest): Promise<WorkspaceDto> => {
        const { data } = await apiClient.put<WorkspaceDto>(`/workspaces/${id}`, request);
        return data;
    },

    getWorkspaceMembers: async (id: string): Promise<WorkspaceMemberDto[]> => {
        const { data } = await apiClient.get<WorkspaceMemberDto[]>(`/workspaces/${id}/members`);
        return data;
    },

    inviteMember: async (workspaceId: string, request: InviteMemberRequest): Promise<void> => {
        await apiClient.post(`/workspaces/${workspaceId}/invite`, request);
    },

    updateMemberRole: async (workspaceId: string, memberId: string, request: UpdateMemberRoleRequest): Promise<void> => {
        await apiClient.put(`/workspaces/${workspaceId}/members/${memberId}/role`, request);
    },

    removeMember: async (workspaceId: string, memberId: string): Promise<void> => {
        await apiClient.delete(`/workspaces/${workspaceId}/members/${memberId}`);
    }
};