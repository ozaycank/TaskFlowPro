import { apiClient } from '@/api/client';
import { TaskDto, TaskDetailDto, CreateTaskCommand, TransitionTaskStateCommand, UpdateTaskCommand } from '../types/task.types';

export const taskApi = {
    getTasksByProject: async (projectId: string): Promise<TaskDto[]> => {
        const response = await apiClient.get<TaskDto[]>(`/tasks/project/${projectId}`);
        return response.data;
    },

    // Phase 26 Addition: Fetch a single task
    getTaskById: async (taskId: string): Promise<TaskDetailDto> => {
        const response = await apiClient.get<TaskDetailDto>(`/tasks/${taskId}`);
        return response.data;
    },

    createTask: async (command: CreateTaskCommand): Promise<string> => {
        const response = await apiClient.post<string>('/tasks', command);
        return response.data;
    },

    // Phase 26 Addition: Update a task
    updateTask: async (command: UpdateTaskCommand): Promise<void> => {
        await apiClient.put(`/tasks/${command.taskId}`, command);
    },

    transitionTask: async (command: TransitionTaskStateCommand): Promise<void> => {
        await apiClient.put(`/tasks/${command.taskId}/transition`, command);
    }
};