import { apiClient } from '@/api/client';
import { SprintDto, CreateSprintCommand, UpdateSprintCommand } from '../types/sprint.types';

export const sprintApi = {
    getSprintsByProject: async (projectId: string): Promise<SprintDto[]> => {
        const response = await apiClient.get<SprintDto[]>(`/sprints/project/${projectId}`);
        return response.data;
    },

    createSprint: async (command: CreateSprintCommand): Promise<string> => {
        const response = await apiClient.post<string>('/sprints', command);
        return response.data;
    },

    updateSprint: async (command: UpdateSprintCommand): Promise<void> => {
        await apiClient.put(`/sprints/${command.sprintId}`, command);
    },

    startSprint: async (sprintId: string): Promise<void> => {
        await apiClient.put(`/sprints/${sprintId}/start`);
    },

    completeSprint: async (sprintId: string): Promise<void> => {
        // Optional payload depending on your CQRS implementation, sending empty body
        await apiClient.put(`/sprints/${sprintId}/complete`, { sprintId });
    }
};