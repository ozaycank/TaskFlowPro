import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { automationApi } from '../api/automation.api';
import { CreateAutomationRuleCommand, ToggleAutomationRuleCommand } from '../types/automation.types';

export const AUTOMATION_KEYS = {
    rules: (workspaceId: string, projectId?: string) => ['automations', 'rules', workspaceId, projectId] as const,
};

export const useAutomationRulesQuery = (workspaceId: string, projectId?: string) => {
    return useQuery({
        queryKey: AUTOMATION_KEYS.rules(workspaceId, projectId),
        queryFn: () => automationApi.getRules(workspaceId, projectId),
        enabled: !!workspaceId,
    });
};

export const useCreateAutomationMutation = (workspaceId: string, projectId?: string) => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (cmd: CreateAutomationRuleCommand) => automationApi.createRule(cmd),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: AUTOMATION_KEYS.rules(workspaceId, projectId) });
        }
    });
};

export const useToggleAutomationMutation = (workspaceId: string, projectId?: string) => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (cmd: ToggleAutomationRuleCommand) => automationApi.toggleRule(cmd),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: AUTOMATION_KEYS.rules(workspaceId, projectId) });
        }
    });
};

export const useDeleteAutomationMutation = (workspaceId: string, projectId?: string) => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (ruleId: string) => automationApi.deleteRule(ruleId),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: AUTOMATION_KEYS.rules(workspaceId, projectId) });
        }
    });
};