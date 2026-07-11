'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { createTaskSchema, CreateTaskFormData } from '../schemas/task.schema';
import { useCreateTaskMutation } from '../hooks/useCreateTaskMutation';
import { useWorkflowStatesQuery } from '@/features/workflows/hooks/useWorkflowStatesQuery';
import { PriorityLevel } from '../types/task.types';
import { Plus } from 'lucide-react';

interface CreateTaskDialogProps {
    workspaceId: string;
    projectId: string;
    defaultStateId?: string; // Optional: To create directly in a specific column
}

export const CreateTaskDialog = ({ workspaceId, projectId, defaultStateId }: CreateTaskDialogProps) => {
    const [open, setOpen] = useState(false);
    const { mutate, isPending } = useCreateTaskMutation(projectId);
    const { data: workflowStates } = useWorkflowStatesQuery(workspaceId);

    const { register, handleSubmit, setValue, formState: { errors }, reset } = useForm<CreateTaskFormData>({
        resolver: zodResolver(createTaskSchema),
        defaultValues: { 
            priority: PriorityLevel.Medium,
            stateId: defaultStateId || ''
        }
    });

    const onSubmit = (data: CreateTaskFormData) => {
        // Find the order index based on existing tasks (simplified for now: put at the end)
        mutate({
            workspaceId,
            projectId,
            title: data.title,
            description: data.description,
            priority: data.priority,
            stateId: data.stateId,
            dueDate: data.dueDate ? new Date(data.dueDate).toISOString() : null,
            orderIndex: 99999, // Backend or a smarter calculation can refine this
        }, {
            onSuccess: () => {
                setOpen(false);
                reset();
            }
        });
    };

    return (
        <Dialog open={open} onOpenChange={setOpen}>
            <DialogTrigger asChild>
                <Button size="sm" variant={defaultStateId ? "ghost" : "default"} className={defaultStateId ? "w-full justify-start text-zinc-500" : ""}>
                    <Plus className="mr-2 h-4 w-4" /> Create Task
                </Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[500px]">
                <DialogHeader>
                    <DialogTitle>New Task</DialogTitle>
                </DialogHeader>
                
                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4 pt-4">
                    <div className="space-y-2">
                        <Label>Title</Label>
                        <Input placeholder="What needs to be done?" {...register('title')} />
                        {errors.title && <span className="text-xs text-red-500">{errors.title.message}</span>}
                    </div>

                    <div className="space-y-2">
                        <Label>Description</Label>
                        <textarea 
                            {...register('description')}
                            className="w-full min-h-[100px] px-3 py-2 border rounded-md dark:bg-zinc-950 dark:border-zinc-800 text-sm focus:outline-none focus:ring-2 focus:ring-zinc-900"
                            placeholder="Add more details..."
                        />
                    </div>

                    <div className="grid grid-cols-2 gap-4">
                        <div className="space-y-2">
                            <Label>Status</Label>
                            <Select onValueChange={(val) => setValue('stateId', val)} defaultValue={defaultStateId}>
                                <SelectTrigger>
                                    <SelectValue placeholder="Select Status" />
                                </SelectTrigger>
                                <SelectContent>
                                    {workflowStates?.map(state => (
                                        <SelectItem key={state.id} value={state.id}>{state.name}</SelectItem>
                                    ))}
                                </SelectContent>
                            </Select>
                            {errors.stateId && <span className="text-xs text-red-500">{errors.stateId.message}</span>}
                        </div>

                        <div className="space-y-2">
                            <Label>Priority</Label>
                            <Select onValueChange={(val) => setValue('priority', Number(val) as PriorityLevel)} defaultValue={PriorityLevel.Medium.toString()}>
                                <SelectTrigger>
                                    <SelectValue placeholder="Select Priority" />
                                </SelectTrigger>
                                <SelectContent>
                                    <SelectItem value={PriorityLevel.Low.toString()}>Low</SelectItem>
                                    <SelectItem value={PriorityLevel.Medium.toString()}>Medium</SelectItem>
                                    <SelectItem value={PriorityLevel.High.toString()}>High</SelectItem>
                                    <SelectItem value={PriorityLevel.Urgent.toString()}>Urgent</SelectItem>
                                </SelectContent>
                            </Select>
                        </div>
                    </div>

                    <div className="space-y-2">
                        <Label>Due Date</Label>
                        <Input type="date" {...register('dueDate')} />
                    </div>

                    <div className="pt-4 flex justify-end">
                        <Button type="submit" disabled={isPending}>
                            {isPending ? 'Creating...' : 'Create Task'}
                        </Button>
                    </div>
                </form>
            </DialogContent>
        </Dialog>
    );
};