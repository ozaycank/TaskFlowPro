'use client';

import { useWorkspacesQuery } from '@/features/workspace/hooks/useWorkspacesQuery';
import { WorkspaceEmptyState } from '@/features/workspace/components/WorkspaceEmptyState';
import { WorkspaceLoadingState } from '@/features/workspace/components/WorkspaceLoadingState';
import { WorkspaceErrorState } from '@/features/workspace/components/WorkspaceErrorState';
import { useWorkspaceStore } from '@/features/workspace/stores/useWorkspaceStore';
import { useRouter } from 'next/navigation';
import { useEffect } from 'react';

export default function WorkspacesIndexPage() {
  const { data: workspaces, isLoading, isError, refetch } = useWorkspacesQuery();
  const { setActiveWorkspace, activeWorkspaceId } = useWorkspaceStore();
  const router = useRouter();

  useEffect(() => {
    // If we have an active workspace, route directly to it.
    if (workspaces && workspaces.length > 0) {
      const targetId = activeWorkspaceId || workspaces[0].id;
      if (!activeWorkspaceId) setActiveWorkspace(targetId);
      router.push(`/workspaces/${targetId}`);
    }
  }, [workspaces, activeWorkspaceId, router, setActiveWorkspace]);

  if (isLoading) return <WorkspaceLoadingState />;
  if (isError) return <WorkspaceErrorState retry={refetch} />;
  
  // Sadece hiç workspace yoksa bu ekran görünür
  return (
    <div className="max-w-2xl mx-auto mt-20">
      <WorkspaceEmptyState onAction={() => {
        // Trigger create modal or navigate to a dedicated create page
        document.getElementById('dialog-trigger-create-workspace')?.click();
      }} />
    </div>
  );
}