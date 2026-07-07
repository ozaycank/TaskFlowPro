'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { useCreateWorklogMutation } from '../hooks/useTimeTrackingHooks';
import { Clock } from 'lucide-react';
import { format } from 'date-fns';

const timeEntrySchema = z.object({
    hours: z.number().min(0).max(24),
    minutes: z.number().min(0).max(59),
    startDate: z.string().min(1, 'Date is required'),
    description: z.string().max(500).optional(),
}).refine(data => data.hours > 0 || data.minutes > 0, {
    message: "Time spent must be greater than 0",
    path: ["minutes"]
});

type FormData = z.infer<typeof timeEntrySchema>;

export const TimeEntryDialog = ({ taskId }: { taskId: string }) => {
    const [open, setOpen] = useState(false);
    const { mutate: createWorklog, isPending } = useCreateWorklogMutation(taskId);

    const { register, handleSubmit, reset, formState: { errors } } = useForm<FormData>({
        resolver: zodResolver(timeEntrySchema),
        defaultValues: {
            hours: 0,
            minutes: 0,
            startDate: format(new Date(), 'yyyy-MM-dd'),
            description: ''
        }
    });

    const onSubmit = (data: FormData) => {
        const timeSpentSeconds = (data.hours * 3600) + (data.minutes * 60);

        createWorklog({
            taskId,
            userId: '00000000-0000-0000-0000-000000000000', // Replace with actual logged-in user ID if backend strictly validates
            timeSpentSeconds,
            startDate: new Date(data.startDate).toISOString(),
            description: data.description
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
                <Button size="sm" variant="outline" className="gap-2">
                    <Clock size={16} /> Log Time
                </Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>Log Work</DialogTitle>
                </DialogHeader>
                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4 pt-4">
                    <div className="grid grid-cols-2 gap-4">
                        <div className="space-y-2">
                            <Label>Hours</Label>
                            <Input type="number" min="0" max="24" {...register('hours', { valueAsNumber: true })} />
                        </div>
                        <div className="space-y-2">
                            <Label>Minutes</Label>
                            <Input type="number" min="0" max="59" step="5" {...register('minutes', { valueAsNumber: true })} />
                            {errors.minutes && <span className="text-xs text-red-500">{errors.minutes.message}</span>}
                        </div>
                    </div>

                    <div className="space-y-2">
                        <Label>Date</Label>
                        <Input type="date" {...register('startDate')} />
                        {errors.startDate && <span className="text-xs text-red-500">{errors.startDate.message}</span>}
                    </div>

                    <div className="space-y-2">
                        <Label>Description</Label>
                        <textarea 
                            {...register('description')}
                            className="w-full min-h-[80px] p-3 border rounded-md dark:bg-zinc-950 dark:border-zinc-800 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
                            placeholder="What did you work on?"
                        />
                    </div>

                    <div className="flex justify-end pt-2">
                        <Button type="submit" disabled={isPending}>
                            {isPending ? 'Saving...' : 'Log Time'}
                        </Button>
                    </div>
                </form>
            </DialogContent>
        </Dialog>
    );
};