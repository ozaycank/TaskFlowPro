export enum AutomationTriggerType {
    TaskCreated = 1,
    TaskStateChanged = 2,
    TaskAssigned = 3,
    TaskPriorityChanged = 4
}

export enum AutomationActionType {
    ChangeState = 1,
    AssignUser = 2,
    AddComment = 3,
    SendNotification = 4
}

export interface AutomationRuleDto {
    id: string;
    workspaceId: string;
    projectId: string | null;
    name: string;
    description: string | null;
    isActive: boolean;
    triggerType: AutomationTriggerType;
    triggerConditionsJson: string | null;
    actionType: AutomationActionType;
    actionPayloadJson: string;
    createdAt: string;
}

export interface CreateAutomationRuleCommand {
    workspaceId: string;
    projectId?: string | null;
    name: string;
    description?: string | null;
    triggerType: AutomationTriggerType;
    triggerConditionsJson?: string | null;
    actionType: AutomationActionType;
    actionPayloadJson: string;
}

export interface ToggleAutomationRuleCommand {
    ruleId: string;
    isActive: boolean;
}