'use client';

import { ProjectGrid } from '@/features/projects/components/ProjectGrid';
import { CreateProjectDialog } from '@/features/projects/components/CreateProjectDialog';
import { useWorkspaceStore } from '@/features/workspace/stores/useWorkspaceStore';

export default function ProjectsIndexPage() {
  const activeWorkspaceId = useWorkspaceStore((state) => state.activeWorkspaceId);

  if (!activeWorkspaceId) {
    return <div className="p-8 text-center text-zinc-500">Please select a workspace first.</div>;
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Projects</h1>
          <p className="text-zinc-500">Manage all initiatives within this workspace.</p>
        </div>
        <CreateProjectDialog />
      </div>
      <ProjectGrid />
    </div>
  );
}