import { apiClient } from '@/api/client';
import {
    CustomFieldDefinitionDto,
    CreateFieldDefinitionCommand,
    UpdateFieldDefinitionCommand,
    SetTaskFieldValueCommand
} from '../types/custom-field.types';

export const customFieldApi = {
    getDefinitions: async (workspaceId: string, projectId?: string): Promise<CustomFieldDefinitionDto[]> => {
        const url = projectId
            ? `/custom-fields/workspaces/${workspaceId}?projectId=${projectId}`
            : `/custom-fields/workspaces/${workspaceId}`;
        const { data } = await apiClient.get<CustomFieldDefinitionDto[]>(url);
        return data;
    },

    createDefinition: async (command: CreateFieldDefinitionCommand): Promise<string> => {
        const { data } = await apiClient.post<string>('/custom-fields', command);
        return data;
    },

    updateDefinition: async (command: UpdateFieldDefinitionCommand): Promise<void> => {
        await apiClient.put(`/custom-fields/${command.id}`, command);
    },

    deleteDefinition: async (id: string): Promise<void> => {
        await apiClient.delete(`/custom-fields/${id}`);
    },

    setTaskValue: async (command: SetTaskFieldValueCommand): Promise<void> => {
        await apiClient.put(`/custom-fields/tasks/${command.taskId}/values`, command);
    }
};