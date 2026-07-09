'use client';

import { useAutomationRulesQuery, useToggleAutomationMutation, useDeleteAutomationMutation } from '../hooks/useAutomationHooks';
import { AutomationTriggerType, AutomationActionType } from '../types/automation.types';
import { CreateAutomationDialog } from './CreateAutomationDialog';
import { Skeleton } from '@/components/ui/skeleton';
import { Trash2, Play, Pause, Zap } from 'lucide-react';

export const AutomationList = ({ workspaceId, projectId }: { workspaceId: string, projectId?: string }) => {
    const { data: rules, isLoading } = useAutomationRulesQuery(workspaceId, projectId);
    const { mutate: toggleRule, isPending: isToggling } = useToggleAutomationMutation(workspaceId, projectId);
    const { mutate: deleteRule, isPending: isDeleting } = useDeleteAutomationMutation(workspaceId, projectId);

    const getTriggerText = (type: AutomationTriggerType) => {
        switch (type) {
            case AutomationTriggerType.TaskCreated: return 'Task is Created';
            case AutomationTriggerType.TaskStateChanged: return 'Task State Changes';
            case AutomationTriggerType.TaskAssigned: return 'Task is Assigned';
            case AutomationTriggerType.TaskPriorityChanged: return 'Task Priority Changes';
            default: return 'Unknown Trigger';
        }
    };

    const getActionText = (type: AutomationActionType) => {
        switch (type) {
            case AutomationActionType.ChangeState: return 'Change Task State';
            case AutomationActionType.AssignUser: return 'Assign User';
            case AutomationActionType.AddComment: return 'Add Comment';
            case AutomationActionType.SendNotification: return 'Send Notification';
            default: return 'Unknown Action';
        }
    };

    return (
        <div className="bg-white dark:bg-zinc-900 border border-zinc-200 dark:border-zinc-800 rounded-xl shadow-sm overflow-hidden">
            <div className="p-6 border-b border-zinc-200 dark:border-zinc-800 flex justify-between items-center bg-zinc-50/50 dark:bg-zinc-950/50">
                <div>
                    <h2 className="text-xl font-semibold dark:text-zinc-100 flex items-center gap-2">
                        <Zap className="text-indigo-500" size={20} /> Workflow Automations
                    </h2>
                    <p className="text-sm text-zinc-500 mt-1">Automate routine tasks and enforce processes.</p>
                </div>
                <CreateAutomationDialog workspaceId={workspaceId} projectId={projectId} />
            </div>

            <div className="p-6">
                {isLoading ? (
                    <div className="space-y-4">
                        <Skeleton className="h-20 w-full rounded-lg" />
                        <Skeleton className="h-20 w-full rounded-lg" />
                    </div>
                ) : rules?.length === 0 ? (
                    <div className="text-center py-12 text-zinc-500 border border-dashed border-zinc-200 dark:border-zinc-800 rounded-lg">
                        <Zap size={32} className="mx-auto mb-3 opacity-20" />
                        <p>No automation rules defined.</p>
                    </div>
                ) : (
                    <div className="space-y-4">
                        {rules?.map(rule => (
                            <div 
                                key={rule.id} 
                                className={`flex items-center justify-between p-4 border rounded-lg transition-colors ${
                                    rule.isActive 
                                    ? 'bg-white dark:bg-zinc-900 border-zinc-200 dark:border-zinc-800' 
                                    : 'bg-zinc-50 dark:bg-zinc-950 border-zinc-100 dark:border-zinc-900 opacity-60'
                                }`}
                            >
                                <div className="flex-1">
                                    <div className="flex items-center gap-3 mb-2">
                                        <h3 className="font-semibold text-zinc-900 dark:text-zinc-100">{rule.name}</h3>
                                        <span className={`text-[10px] px-2 py-0.5 rounded-full font-bold uppercase tracking-wider ${rule.isActive ? 'bg-emerald-100 text-emerald-700 dark:bg-emerald-900/30 dark:text-emerald-400' : 'bg-zinc-200 text-zinc-600 dark:bg-zinc-800 dark:text-zinc-400'}`}>
                                            {rule.isActive ? 'Active' : 'Paused'}
                                        </span>
                                    </div>
                                    <div className="flex items-center text-sm">
                                        <span className="font-medium text-indigo-600 dark:text-indigo-400 mr-2">WHEN</span>
                                        <span className="text-zinc-600 dark:text-zinc-300">{getTriggerText(rule.triggerType)}</span>
                                        
                                        <span className="mx-3 text-zinc-300 dark:text-zinc-700">→</span>
                                        
                                        <span className="font-medium text-emerald-600 dark:text-emerald-400 mr-2">THEN</span>
                                        <span className="text-zinc-600 dark:text-zinc-300">{getActionText(rule.actionType)}</span>
                                    </div>
                                </div>

                                <div className="flex items-center gap-2 ml-4">
                                    <button
                                        onClick={() => toggleRule({ ruleId: rule.id, isActive: !rule.isActive })}
                                        disabled={isToggling}
                                        className="p-2 rounded-md bg-zinc-100 hover:bg-zinc-200 dark:bg-zinc-800 dark:hover:bg-zinc-700 text-zinc-600 dark:text-zinc-300 transition-colors"
                                        title={rule.isActive ? "Pause Rule" : "Activate Rule"}
                                    >
                                        {rule.isActive ? <Pause size={16} /> : <Play size={16} />}
                                    </button>
                                    <button
                                        onClick={() => { if(confirm('Delete this automation rule?')) deleteRule(rule.id) }}
                                        disabled={isDeleting}
                                        className="p-2 rounded-md hover:bg-red-50 hover:text-red-600 dark:hover:bg-red-900/20 dark:hover:text-red-400 text-zinc-400 transition-colors"
                                    >
                                        <Trash2 size={16} />
                                    </button>
                                </div>
                            </div>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
};