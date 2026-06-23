'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { createProjectSchema, CreateProjectFormData } from '../schemas/project.schema';
import { useCreateProjectMutation } from '../hooks/useCreateProjectMutation';
import { useWorkspaceStore } from '@/features/workspace/stores/useWorkspaceStore';
import { useProjectStore } from '../stores/useProjectStore';
import { useEntitlement, EntitlementFeature } from '@/features/billing/hooks/useEntitlement';
import { useProjectsQuery } from '../hooks/useProjectsQuery';
import { Plus } from 'lucide-react';
import { useRouter } from 'next/navigation';

export const CreateProjectDialog = () => {
  const [open, setOpen] = useState(false);
  const router = useRouter();
  const activeWorkspaceId = useWorkspaceStore((state) => state.activeWorkspaceId);
  const setActiveProject = useProjectStore((state) => state.setActiveProject);
  const { data: projects } = useProjectsQuery();
  const { mutate, isPending } = useCreateProjectMutation();
  
  // Entitlement Check Logic
  const canCreateUnlimited = useEntitlement(EntitlementFeature.MaxActiveProjects);
  const isLimitReached = !canCreateUnlimited && (projects?.length || 0) >= 3;

  const { register, handleSubmit, formState: { errors }, reset } = useForm<CreateProjectFormData>({
    resolver: zodResolver(createProjectSchema),
    defaultValues: { color: '#3b82f6' }
  });

  const onSubmit = (data: CreateProjectFormData) => {
    if (!activeWorkspaceId) return;
    
    mutate({ ...data, workspaceId: activeWorkspaceId }, {
      onSuccess: (newProject) => {
        setOpen(false);
        reset();
        setActiveProject(newProject.id);
        router.push(`/projects/${newProject.id}`);
      },
    });
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button size="sm">
          <Plus className="mr-2 h-4 w-4" /> New Project
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px]">
        <DialogHeader>
          <DialogTitle>Create New Project</DialogTitle>
        </DialogHeader>
        
        {isLimitReached ? (
          <div className="py-6 text-center">
            <h3 className="text-lg font-semibold mb-2">Project Limit Reached</h3>
            <p className="text-sm text-zinc-500 mb-4">Your current plan limits you to 3 active projects. Upgrade to Pro for unlimited projects.</p>
            <Button className="w-full bg-indigo-600 hover:bg-indigo-700 text-white">Upgrade to Pro</Button>
          </div>
        ) : (
          <form onSubmit={handleSubmit(onSubmit)} className="space-y-4 pt-4">
            <div className="space-y-2">
              <Label>Project Name</Label>
              <Input placeholder="e.g. Q3 Marketing Campaign" {...register('name')} />
              {errors.name && <span className="text-xs text-red-500">{errors.name.message}</span>}
            </div>
            <div className="space-y-2">
              <Label>Description</Label>
              <Input placeholder="Brief description..." {...register('description')} />
            </div>
            <div className="space-y-2">
              <Label>Project Color (Hex)</Label>
              <Input type="color" {...register('color')} className="h-10 w-full cursor-pointer" />
            </div>
            <Button type="submit" disabled={isPending} className="w-full">
              {isPending ? 'Creating...' : 'Create Project'}
            </Button>
          </form>
        )}
      </DialogContent>
    </Dialog>
  );
};