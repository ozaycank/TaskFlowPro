'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { useCreateAutomationMutation } from '../hooks/useAutomationHooks';
import { AutomationTriggerType, AutomationActionType } from '../types/automation.types';
import { Zap } from 'lucide-react';

const schema = z.object({
    name: z.string().min(2, 'Name required').max(200),
    description: z.string().max(1000).optional(),
    triggerType: z.nativeEnum(AutomationTriggerType),
    actionType: z.nativeEnum(AutomationActionType),
    // Simplified JSON payload inputs for MVP builder
    targetStateId: z.string().optional(),
    assigneeId: z.string().optional(),
});

type FormData = z.infer<typeof schema>;

export const CreateAutomationDialog = ({ workspaceId, projectId }: { workspaceId: string, projectId?: string }) => {
    const [open, setOpen] = useState(false);
    const { mutate, isPending } = useCreateAutomationMutation(workspaceId, projectId);

    const { register, handleSubmit, watch, reset, formState: { errors } } = useForm<FormData>({
        resolver: zodResolver(schema),
        defaultValues: { 
            triggerType: AutomationTriggerType.TaskCreated, 
            actionType: AutomationActionType.AssignUser 
        }
    });

    const selectedAction = watch('actionType');

    const onSubmit = (data: FormData) => {
        // Construct the JSON payload based on the selected action
        let payload = {};
        if (data.actionType == AutomationActionType.ChangeState && data.targetStateId) {
            payload = { stateId: data.targetStateId };
        } else if (data.actionType == AutomationActionType.AssignUser && data.assigneeId) {
            payload = { assigneeId: data.assigneeId };
        } else {
            payload = { message: "Automated action executed." };
        }

        mutate({
            workspaceId,
            projectId: projectId || null,
            name: data.name,
            description: data.description,
            triggerType: Number(data.triggerType),
            triggerConditionsJson: null, // MVP: No complex conditions yet
            actionType: Number(data.actionType),
            actionPayloadJson: JSON.stringify(payload)
        }, {
            onSuccess: () => { setOpen(false); reset(); }
        });
    };

    return (
        <Dialog open={open} onOpenChange={setOpen}>
            <DialogTrigger asChild>
                <Button size="sm" className="gap-2"><Zap size={16} /> New Automation</Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[500px]">
                <DialogHeader>
                    <DialogTitle>Create Workflow Automation</DialogTitle>
                </DialogHeader>
                <form onSubmit={handleSubmit(onSubmit)} className="space-y-5 pt-4">
                    <div className="space-y-2">
                        <Label>Rule Name</Label>
                        <Input {...register('name')} placeholder="e.g. Auto-assign new tasks" />
                        {errors.name && <span className="text-xs text-red-500">{errors.name.message}</span>}
                    </div>

                    <div className="p-4 border border-zinc-200 dark:border-zinc-800 rounded-lg bg-zinc-50 dark:bg-zinc-900/50 space-y-4">
                        <div className="space-y-2">
                            <Label className="text-indigo-600 dark:text-indigo-400 font-semibold">WHEN (Trigger)</Label>
                            <select {...register('triggerType')} className="flex h-9 w-full rounded-md border border-zinc-200 bg-white px-3 py-1 text-sm shadow-sm dark:bg-zinc-950 dark:border-zinc-800">
                                <option value={AutomationTriggerType.TaskCreated}>Task is Created</option>
                                <option value={AutomationTriggerType.TaskStateChanged}>Task State Changes</option>
                                <option value={AutomationTriggerType.TaskAssigned}>Task is Assigned</option>
                                <option value={AutomationTriggerType.TaskPriorityChanged}>Task Priority Changes</option>
                            </select>
                        </div>

                        <div className="space-y-2 pt-2 border-t border-zinc-200 dark:border-zinc-800">
                            <Label className="text-emerald-600 dark:text-emerald-400 font-semibold">THEN (Action)</Label>
                            <select {...register('actionType')} className="flex h-9 w-full rounded-md border border-zinc-200 bg-white px-3 py-1 text-sm shadow-sm dark:bg-zinc-950 dark:border-zinc-800">
                                <option value={AutomationActionType.ChangeState}>Change Task State</option>
                                <option value={AutomationActionType.AssignUser}>Assign User</option>
                                <option value={AutomationActionType.AddComment}>Add Automated Comment</option>
                            </select>
                        </div>

                        {/* Dynamic Payload Fields for MVP */}
                        {selectedAction == AutomationActionType.ChangeState && (
                            <div className="space-y-2">
                                <Label className="text-xs">Target State ID</Label>
                                <Input {...register('targetStateId')} placeholder="Enter State UUID" />
                            </div>
                        )}
                        {selectedAction == AutomationActionType.AssignUser && (
                            <div className="space-y-2">
                                <Label className="text-xs">Target User ID</Label>
                                <Input {...register('assigneeId')} placeholder="Enter User UUID" />
                            </div>
                        )}
                    </div>

                    <Button type="submit" disabled={isPending} className="w-full">
                        {isPending ? 'Saving...' : 'Create Rule'}
                    </Button>
                </form>
            </DialogContent>
        </Dialog>
    );
};