'use client';

import { ProjectGrid } from '@/features/projects/components/ProjectGrid';
import { CreateProjectDialog } from '@/features/projects/components/CreateProjectDialog';
import { useWorkspaceStore } from '@/features/workspace/stores/useWorkspaceStore';
import { useParams } from 'next/navigation';
import { useEffect } from 'react';

export default function WorkspaceProjectsPage() {
  const params = useParams();
  const workspaceId = params.workspaceId as string;
  const setActiveWorkspace = useWorkspaceStore((state) => state.setActiveWorkspace);

  // Ensure the store knows which workspace we are looking at
  useEffect(() => {
    if (workspaceId) setActiveWorkspace(workspaceId);
  }, [workspaceId, setActiveWorkspace]);

  return (
    <div className="space-y-6 max-w-7xl mx-auto">
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Projects</h1>
          <p className="text-zinc-500">Manage all initiatives within this workspace.</p>
        </div>
        <CreateProjectDialog />
      </div>
      
      {/* This component will fetch and display projects for the active workspace */}
      <ProjectGrid />
    </div>
  );
}