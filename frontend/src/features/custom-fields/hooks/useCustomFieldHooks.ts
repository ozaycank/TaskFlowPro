import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { customFieldApi } from '../api/custom-field.api';
import { CreateFieldDefinitionCommand, UpdateFieldDefinitionCommand, SetTaskFieldValueCommand } from '../types/custom-field.types';
import { TASK_DETAIL_QUERY_KEY } from '@/features/tasks/hooks/useTaskQuery';

export const CUSTOM_FIELD_KEYS = {
    definitions: (workspaceId: string, projectId?: string) => ['custom-fields', 'definitions', workspaceId, projectId] as const,
};

export const useFieldDefinitionsQuery = (workspaceId: string, projectId?: string) => {
    return useQuery({
        queryKey: CUSTOM_FIELD_KEYS.definitions(workspaceId, projectId),
        queryFn: () => customFieldApi.getDefinitions(workspaceId, projectId),
        enabled: !!workspaceId,
    });
};

export const useCreateFieldDefinitionMutation = (workspaceId: string, projectId?: string) => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (cmd: CreateFieldDefinitionCommand) => customFieldApi.createDefinition(cmd),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: CUSTOM_FIELD_KEYS.definitions(workspaceId, projectId) });
        }
    });
};

export const useDeleteFieldDefinitionMutation = (workspaceId: string, projectId?: string) => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (id: string) => customFieldApi.deleteDefinition(id),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: CUSTOM_FIELD_KEYS.definitions(workspaceId, projectId) });
        }
    });
};

export const useSetTaskFieldValueMutation = (taskId: string) => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (cmd: SetTaskFieldValueCommand) => customFieldApi.setTaskValue(cmd),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: TASK_DETAIL_QUERY_KEY(taskId) });
        }
    });
};