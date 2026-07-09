import { apiClient } from '@/api/client';
import {
    AutomationRuleDto,
    CreateAutomationRuleCommand,
    ToggleAutomationRuleCommand
} from '../types/automation.types';

export const automationApi = {
    getRules: async (workspaceId: string, projectId?: string): Promise<AutomationRuleDto[]> => {
        const url = projectId
            ? `/automations/workspaces/${workspaceId}?projectId=${projectId}`
            : `/automations/workspaces/${workspaceId}`;
        const { data } = await apiClient.get<AutomationRuleDto[]>(url);
        return data;
    },

    createRule: async (command: CreateAutomationRuleCommand): Promise<string> => {
        const { data } = await apiClient.post<string>('/automations', command);
        return data;
    },

    toggleRule: async (command: ToggleAutomationRuleCommand): Promise<void> => {
        await apiClient.patch(`/automations/${command.ruleId}/toggle`, command);
    },

    deleteRule: async (ruleId: string): Promise<void> => {
        await apiClient.delete(`/automations/${ruleId}`);
    }
};