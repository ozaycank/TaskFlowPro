import { apiClient } from '@/api/client';
import { TaskDto, CreateTaskCommand, TransitionTaskStateCommand } from '../types/task.types';

export const taskApi = {
    getTasksByProject: async (projectId: string): Promise<TaskDto[]> => {
        const response = await apiClient.get<TaskDto[]>(`/tasks/project/${projectId}`);
        return response.data;
    },
    createTask: async (command: CreateTaskCommand): Promise<string> => {
        const response = await apiClient.post<string>('/tasks', command);
        return response.data;
    },
    transitionTask: async (command: TransitionTaskStateCommand): Promise<void> => {
        await apiClient.put(`/tasks/${command.taskId}/transition`, command);
    }
};