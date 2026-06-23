import { apiClient } from '@/api/client';
import { CreateProjectRequest, ProjectDto, ProjectMemberDto, UpdateProjectRequest } from '../types/project.types';

export const projectApi = {
    getProjectsByWorkspace: async (workspaceId: string): Promise<ProjectDto[]> => {
        const { data } = await apiClient.get<ProjectDto[]>(`/projects?workspaceId=${workspaceId}`);
        return data;
    },

    getProjectById: async (id: string): Promise<ProjectDto> => {
        const { data } = await apiClient.get<ProjectDto>(`/projects/${id}`);
        return data;
    },

    createProject: async (request: CreateProjectRequest): Promise<ProjectDto> => {
        const { data } = await apiClient.post<ProjectDto>('/projects', request);
        return data;
    },

    updateProject: async (id: string, request: UpdateProjectRequest): Promise<ProjectDto> => {
        const { data } = await apiClient.put<ProjectDto>(`/projects/${id}`, request);
        return data;
    },

    deleteProject: async (id: string): Promise<void> => {
        await apiClient.delete(`/projects/${id}`);
    },

    getProjectMembers: async (id: string): Promise<ProjectMemberDto[]> => {
        const { data } = await apiClient.get<ProjectMemberDto[]>(`/projects/${id}/members`);
        return data;
    },
};