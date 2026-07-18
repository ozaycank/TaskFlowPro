'use client';

import { useParams, useRouter } from 'next/navigation';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useState, useEffect } from 'react';
import { useTaskQuery } from '@/features/tasks/hooks/useTaskQuery';
import { useUpdateTaskMutation } from '@/features/tasks/hooks/useUpdateTaskMutation';
import { useDeleteTaskMutation } from '@/features/tasks/hooks/useDeleteTaskMutation';
import { updateTaskSchema, UpdateTaskFormData } from '@/features/tasks/schemas/task.schema';
import { PriorityLevel } from '@/features/tasks/types/task.types';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Skeleton } from '@/components/ui/skeleton';
import { ArrowLeft, Save, Trash2 } from 'lucide-react';
import { UploadDropzone } from '@/features/tasks/components/UploadDropzone';
import { AttachmentList } from '@/features/tasks/components/AttachmentList';
import { CommentList } from '@/features/tasks/components/CommentList';
import { CommentEditor } from '@/features/tasks/components/CommentEditor';
import { TimeEntryDialog } from '@/features/time-tracking/components/TimeEntryDialog';
import { WorklogList } from '@/features/time-tracking/components/WorklogList';
import { TaskCustomFieldsSection } from '@/features/custom-fields/components/TaskCustomFieldsSection';

export default function TaskDetailsPage() {
    const params = useParams();
    const router = useRouter();
    const workspaceId = params.workspaceId as string;
    const projectId = params.projectId as string;
    const taskId = params.taskId as string;

    const { data: task, isLoading } = useTaskQuery(taskId);
    const { mutate: updateTask, isPending: isUpdating } = useUpdateTaskMutation(projectId, taskId);
    const { mutateAsync: deleteTask, isPending: isDeleting } = useDeleteTaskMutation(projectId);

    const [deleteError, setDeleteError] = useState<string | null>(null);

    const { register, handleSubmit, setValue, reset, formState: { isDirty } } = useForm<UpdateTaskFormData>({
        resolver: zodResolver(updateTaskSchema)
    });

    // Populate form when task data arrives
    useEffect(() => {
        if (task) {
            reset({
                title: task.title,
                description: task.description || '',
                priority: task.priority,
                dueDate: task.dueDate ? new Date(task.dueDate).toISOString().split('T')[0] : null,
            });
        }
    }, [task, reset]);

    const onSubmit = (data: UpdateTaskFormData) => {
        updateTask({
            taskId,
            title: data.title,
            description: data.description,
            priority: data.priority,
            dueDate: data.dueDate ? new Date(data.dueDate).toISOString() : null,
        });
    };

    const handleDelete = async () => {
        if (!window.confirm('Are you sure you want to delete this task? This action cannot be undone.')) {
            return;
        }

        setDeleteError(null);
        try {
            await deleteTask(taskId);
            router.push(`/workspaces/${workspaceId}/projects/${projectId}`);
        } catch (err: any) {
            console.error('Failed to delete task:', err);
            setDeleteError(err.response?.data?.message || 'Failed to delete task. Check your permissions.');
        }
    };

    if (isLoading) return <Skeleton className="w-full h-[500px] rounded-xl" />;
    if (!task) return <div className="p-8 text-center text-zinc-500">Task not found.</div>;

    return (
        <div className="max-w-3xl mx-auto space-y-6">
            <div className="flex items-center justify-between mb-4">
                <Button variant="ghost" onClick={() => router.push(`/workspaces/${workspaceId}/projects/${projectId}`)}>
                    <ArrowLeft className="mr-2 h-4 w-4" /> Back to Board
                </Button>

                {/* PHASE 6: Added Delete Button securely */}
                <Button variant="destructive" size="sm" onClick={handleDelete} disabled={isDeleting}>
                    <Trash2 size={16} className="mr-2" />
                    {isDeleting ? 'Deleting...' : 'Delete Task'}
                </Button>
            </div>

            {deleteError && (
                <div className="p-4 bg-red-50 dark:bg-red-900/20 text-red-600 dark:text-red-400 rounded-lg text-sm border border-red-200 dark:border-red-900/30">
                    {deleteError}
                </div>
            )}

            <div className="bg-white dark:bg-zinc-900 border border-zinc-200 dark:border-zinc-800 rounded-xl p-6 shadow-sm">
                <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
                    
                    <div className="space-y-2">
                        <Label className="text-xs text-zinc-500 uppercase tracking-wider">Task Title</Label>
                        <Input 
                            className="text-2xl font-bold border-0 px-0 focus-visible:ring-0 rounded-none border-b focus-visible:border-b-indigo-500 bg-transparent" 
                            {...register('title')} 
                        />
                    </div>

                    <div className="grid grid-cols-2 gap-6 pt-4">
                        <div className="space-y-2">
                            <Label>Priority</Label>
                            <Select onValueChange={(val) => setValue('priority', Number(val) as PriorityLevel)} defaultValue={task.priority.toString()}>
                                <SelectTrigger>
                                    <SelectValue placeholder="Select Priority" />
                                </SelectTrigger>
                                <SelectContent>
                                    <SelectItem value={PriorityLevel.Low.toString()}>Low</SelectItem>
                                    <SelectItem value={PriorityLevel.Medium.toString()}>Medium</SelectItem>
                                    <SelectItem value={PriorityLevel.High.toString()}>High</SelectItem>
                                    <SelectItem value={PriorityLevel.Urgent.toString()}>Urgent</SelectItem>
                                    <SelectItem value={PriorityLevel.Critical.toString()}>Critical</SelectItem>
                                </SelectContent>
                            </Select>
                        </div>

                        <div className="space-y-2">
                            <Label>Due Date</Label>
                            <Input type="date" {...register('dueDate')} />
                        </div>
                    </div>

                    <div className="space-y-2 pt-4">
                        <Label>Description</Label>
                        <textarea 
                            {...register('description')}
                            className="w-full min-h-[200px] p-4 border rounded-md dark:bg-zinc-950 dark:border-zinc-800 focus:outline-none focus:ring-2 focus:ring-indigo-500"
                        />
                    </div>

                    {task && task.customFieldsData && (
                        <TaskCustomFieldsSection 
                            taskId={taskId} 
                            workspaceId={workspaceId}
                            projectId={task.projectId} 
                            customFieldsData={task.customFieldsData} 
                        />
                    )}

                    {isDirty && (
                        <div className="flex justify-end pt-4 border-t dark:border-zinc-800">
                            <Button type="submit" disabled={isUpdating}>
                                <Save className="w-4 h-4 mr-2" />
                                {isUpdating ? 'Saving...' : 'Save Changes'}
                            </Button>
                        </div>
                    )}
                </form>
            </div>

            <div className="bg-white dark:bg-zinc-900 border border-zinc-200 dark:border-zinc-800 rounded-xl shadow-sm overflow-hidden">
                <div className="border-b border-zinc-200 dark:border-zinc-800 px-6 py-4">
                    <h3 className="font-semibold dark:text-white">Attachments</h3>
                </div>
                <div className="p-6">
                    <UploadDropzone taskId={taskId} />
                    <AttachmentList taskId={taskId} />
                </div>
            </div>

            <div className="bg-white dark:bg-zinc-900 border border-zinc-200 dark:border-zinc-800 rounded-xl shadow-sm overflow-hidden mb-12">
                <div className="border-b border-zinc-200 dark:border-zinc-800 px-6 py-4">
                    <h3 className="font-semibold dark:text-white">Discussion</h3>
                </div>
                <div className="p-6 space-y-8">
                    <CommentList taskId={taskId} />
                    <CommentEditor taskId={taskId} />
                </div>
            </div>

            {/* --- PHASE 31: ENTERPRISE TIME TRACKING --- */}
            <div className="bg-white dark:bg-zinc-900 border border-zinc-200 dark:border-zinc-800 rounded-xl shadow-sm overflow-hidden mb-6">
                <div className="border-b border-zinc-200 dark:border-zinc-800 px-6 py-4 flex justify-between items-center">
                    <h3 className="font-semibold dark:text-white">Time Tracking</h3>
                    <TimeEntryDialog taskId={taskId} />
                </div>
                <div className="p-6">
                    <WorklogList taskId={taskId} />
                </div>
            </div>
        </div>
    );
}