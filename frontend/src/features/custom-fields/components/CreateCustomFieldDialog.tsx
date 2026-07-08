'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { useCreateFieldDefinitionMutation } from '../hooks/useCustomFieldHooks';
import { FieldType } from '../types/custom-field.types';
import { Plus } from 'lucide-react';

const schema = z.object({
    name: z.string().min(2, 'Name required').max(100),
    type: z.nativeEnum(FieldType),
    options: z.string().optional(),
    isRequired: z.boolean()
});

type FormData = z.infer<typeof schema>;

export const CreateCustomFieldDialog = ({ workspaceId }: { workspaceId: string }) => {
    const [open, setOpen] = useState(false);
    const { mutate, isPending } = useCreateFieldDefinitionMutation(workspaceId);

    const { register, handleSubmit, watch, reset, formState: { errors } } = useForm<FormData>({
        resolver: zodResolver(schema),
        defaultValues: { type: FieldType.Text, isRequired: false, options: '' }
    });

    const selectedType = watch('type');

    const onSubmit = (data: FormData) => {
        const optionsJson = data.type == FieldType.SingleSelect && data.options 
            ? JSON.stringify(data.options.split(',').map(s => s.trim())) 
            : null;

        mutate({
            workspaceId,
            name: data.name,
            type: Number(data.type),
            optionsJson,
            isRequired: data.isRequired
        }, {
            onSuccess: () => { setOpen(false); reset(); }
        });
    };

    return (
        <Dialog open={open} onOpenChange={setOpen}>
            <DialogTrigger asChild>
                <Button size="sm"><Plus size={16} className="mr-2" /> Add Custom Field</Button>
            </DialogTrigger>
            <DialogContent>
                <DialogHeader>
                    <DialogTitle>Create Custom Field</DialogTitle>
                </DialogHeader>
                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4 pt-4">
                    <div className="space-y-2">
                        <Label>Field Name</Label>
                        <Input {...register('name')} placeholder="e.g. Estimated Budget" />
                        {errors.name && <span className="text-xs text-red-500">{errors.name.message}</span>}
                    </div>

                    <div className="space-y-2">
                        <Label>Field Type</Label>
                        <select {...register('type')} className="flex h-9 w-full rounded-md border border-zinc-200 bg-transparent px-3 py-1 text-sm shadow-sm dark:border-zinc-800">
                            <option value={FieldType.Text}>Text</option>
                            <option value={FieldType.Number}>Number</option>
                            <option value={FieldType.Date}>Date</option>
                            <option value={FieldType.Boolean}>Checkbox</option>
                            <option value={FieldType.SingleSelect}>Dropdown</option>
                            <option value={FieldType.Url}>URL</option>
                        </select>
                    </div>

                    {selectedType == FieldType.SingleSelect && (
                        <div className="space-y-2">
                            <Label>Dropdown Options (comma separated)</Label>
                            <Input {...register('options')} placeholder="Option 1, Option 2, Option 3" />
                        </div>
                    )}

                    <div className="flex items-center gap-2 mt-4">
                        <input type="checkbox" id="isRequired" {...register('isRequired')} />
                        <Label htmlFor="isRequired">This field is required</Label>
                    </div>

                    <Button type="submit" disabled={isPending} className="w-full mt-4">
                        {isPending ? 'Creating...' : 'Create Field'}
                    </Button>
                </form>
            </DialogContent>
        </Dialog>
    );
};