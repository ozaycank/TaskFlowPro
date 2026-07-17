'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { createWorkspaceSchema, CreateWorkspaceFormData } from '../schemas/workspace.schema';
import { useCreateWorkspaceMutation } from '../hooks/useCreateWorkspaceMutation';
import { useWorkspaceStore } from '../stores/useWorkspaceStore';
import { Plus } from 'lucide-react';

// PHASE 4 FIX: Added optional trigger prop for custom trigger buttons
interface CreateWorkspaceDialogProps {
  trigger?: React.ReactNode;
}

export const CreateWorkspaceDialog = ({ trigger }: CreateWorkspaceDialogProps) => {
  const [open, setOpen] = useState(false);
  const { mutate, isPending } = useCreateWorkspaceMutation();
  const setActiveWorkspace = useWorkspaceStore((state) => state.setActiveWorkspace);

  const { register, handleSubmit, formState: { errors }, reset } = useForm<CreateWorkspaceFormData>({
    resolver: zodResolver(createWorkspaceSchema),
  });

  const onSubmit = (data: CreateWorkspaceFormData) => {
    mutate(data, {
      onSuccess: (newWorkspace) => {
        setOpen(false);
        reset();
        setActiveWorkspace(newWorkspace.id); // Otomatik yeni olana geç
      },
    });
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        {trigger ? (
          trigger
        ) : (
          <Button variant="outline" className="w-full justify-start mt-4">
            <Plus className="mr-2 h-4 w-4" /> New Workspace
          </Button>
        )}
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px]">
        <DialogHeader>
          <DialogTitle>Create Workspace</DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4 pt-4">
          <div className="space-y-2">
            <Label htmlFor="name">Workspace Name</Label>
            <Input id="name" placeholder="Acme Corp" {...register('name')} />
            {errors.name && <span className="text-xs text-red-500">{errors.name.message}</span>}
          </div>
          <div className="space-y-2">
            <Label htmlFor="description">Description (Optional)</Label>
            <Input id="description" placeholder="For the engineering team" {...register('description')} />
          </div>
          <Button type="submit" disabled={isPending} className="w-full">
            {isPending ? 'Creating...' : 'Create Workspace'}
          </Button>
        </form>
      </DialogContent>
    </Dialog>
  );
};