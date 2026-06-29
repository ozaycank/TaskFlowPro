'use client';

import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { useUpdateWorkspaceMutation } from '../hooks/useWorkspaceSettings';
import { Save } from 'lucide-react';
import { WorkspaceDetailDto } from '../api/workspace-settings.api';

const updateWorkspaceSchema = z.object({
    name: z.string().min(2, 'Workspace name must be at least 2 characters.').max(100),
    description: z.string().max(500).optional().nullable(),
});

type FormData = z.infer<typeof updateWorkspaceSchema>;

interface Props {
    workspace: WorkspaceDetailDto;
}

export const WorkspaceSettingsForm = ({ workspace }: Props) => {
    const { mutate: updateWorkspace, isPending } = useUpdateWorkspaceMutation(workspace.id);

    const { register, handleSubmit, reset, formState: { errors, isDirty } } = useForm<FormData>({
        resolver: zodResolver(updateWorkspaceSchema),
        defaultValues: {
            name: workspace.name,
            description: workspace.description || '',
        }
    });

    useEffect(() => {
        reset({ name: workspace.name, description: workspace.description || '' });
    }, [workspace, reset]);

    const onSubmit = (data: FormData) => {
        updateWorkspace({
            workspaceId: workspace.id,
            name: data.name,
            description: data.description || undefined,
        }, {
            onSuccess: () => {
                reset(data); // isDirty state'ini sıfırla
            }
        });
    };

    return (
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
            <div className="space-y-2">
                <Label htmlFor="name">Workspace Name</Label>
                <Input id="name" {...register('name')} className="max-w-md" />
                {errors.name && <span className="text-xs text-red-500">{errors.name.message}</span>}
            </div>

            <div className="space-y-2">
                <Label htmlFor="description">Description</Label>
                <textarea 
                    id="description"
                    {...register('description')}
                    className="w-full max-w-md min-h-[100px] px-3 py-2 border rounded-md dark:bg-zinc-950 dark:border-zinc-800 text-sm focus:outline-none focus:ring-2 focus:ring-zinc-900"
                    placeholder="Brief description of your workspace..."
                />
                {errors.description && <span className="text-xs text-red-500">{errors.description.message}</span>}
            </div>

            <Button type="submit" disabled={!isDirty || isPending}>
                <Save className="w-4 h-4 mr-2" />
                {isPending ? 'Saving...' : 'Save Changes'}
            </Button>
        </form>
    );
};