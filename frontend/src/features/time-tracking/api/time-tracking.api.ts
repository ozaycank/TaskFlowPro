import { apiClient } from '@/api/client';
import { WorklogDto, CreateWorklogCommand, UpdateWorklogCommand } from '../types/time-tracking.types';

export const timeTrackingApi = {
    getWorklogsByTask: async (taskId: string): Promise<WorklogDto[]> => {
        const { data } = await apiClient.get<WorklogDto[]>(`/time-tracking/tasks/${taskId}/worklogs`);
        return data;
    },

    createWorklog: async (command: CreateWorklogCommand): Promise<string> => {
        const { data } = await apiClient.post<string>(`/time-tracking/tasks/${command.taskId}/worklogs`, command);
        return data;
    },

    updateWorklog: async (command: UpdateWorklogCommand): Promise<void> => {
        await apiClient.put(`/time-tracking/worklogs/${command.worklogId}`, command);
    },

    deleteWorklog: async (worklogId: string): Promise<void> => {
        await apiClient.delete(`/time-tracking/worklogs/${worklogId}`);
    }
};