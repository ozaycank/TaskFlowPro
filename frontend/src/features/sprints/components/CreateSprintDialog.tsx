'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { useSprintMutations } from '../hooks/useSprintMutations';
import { Plus } from 'lucide-react';

const sprintSchema = z.object({
    name: z.string().min(2, 'Name required').max(200),
    goal: z.string().max(1000).optional(),
    startDate: z.string().optional(),
    endDate: z.string().optional()
});

type FormData = z.infer<typeof sprintSchema>;

export const CreateSprintDialog = ({ projectId }: { projectId: string }) => {
    const [open, setOpen] = useState(false);
    const { createMutation } = useSprintMutations(projectId);

    const { register, handleSubmit, reset, formState: { errors } } = useForm<FormData>({
        resolver: zodResolver(sprintSchema)
    });

    const onSubmit = (data: FormData) => {
        createMutation.mutate({
            projectId,
            name: data.name,
            goal: data.goal,
            startDate: data.startDate ? new Date(data.startDate).toISOString() : null,
            endDate: data.endDate ? new Date(data.endDate).toISOString() : null,
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
                <Button size="sm">
                    <Plus className="mr-2 h-4 w-4" /> Create Sprint
                </Button>
            </DialogTrigger>
            <DialogContent>
                <DialogHeader>
                    <DialogTitle>Create New Sprint</DialogTitle>
                </DialogHeader>
                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4 pt-4">
                    <div className="space-y-2">
                        <Label>Sprint Name</Label>
                        <Input placeholder="e.g. Sprint 1" {...register('name')} />
                        {errors.name && <span className="text-xs text-red-500">{errors.name.message}</span>}
                    </div>
                    <div className="space-y-2">
                        <Label>Sprint Goal</Label>
                        <Input placeholder="What do we want to achieve?" {...register('goal')} />
                    </div>
                    <div className="grid grid-cols-2 gap-4">
                        <div className="space-y-2">
                            <Label>Start Date</Label>
                            <Input type="date" {...register('startDate')} />
                        </div>
                        <div className="space-y-2">
                            <Label>End Date</Label>
                            <Input type="date" {...register('endDate')} />
                        </div>
                    </div>
                    <Button type="submit" disabled={createMutation.isPending} className="w-full">
                        {createMutation.isPending ? 'Creating...' : 'Create Sprint'}
                    </Button>
                </form>
            </DialogContent>
        </Dialog>
    );
};