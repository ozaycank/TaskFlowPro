'use client';

import { useParams } from 'next/navigation';
import { useProjectQuery } from '@/features/projects/hooks/useProjectQuery';
import { Skeleton } from '@/components/ui/skeleton';
import { useEffect } from 'react';
import { useProjectStore } from '@/features/projects/stores/useProjectStore';

export default function ProjectDashboardPage() {
  const params = useParams();
  const projectId = params.projectId as string;
  const { data: project, isLoading } = useProjectQuery(projectId);
  const setActiveProject = useProjectStore((state) => state.setActiveProject);

  useEffect(() => {
    setActiveProject(projectId);
  }, [projectId, setActiveProject]);

  if (isLoading) return <Skeleton className="h-10 w-[300px]" />;
  if (!project) return <div>Project not found.</div>;

  return (
    <div className="space-y-6">
      <div className="flex items-center gap-3">
        <div className="w-6 h-6 rounded-md" style={{ backgroundColor: project.color }} />
        <h1 className="text-3xl font-bold tracking-tight">{project.name}</h1>
      </div>
      <p className="text-zinc-500">{project.description}</p>
      
      <div className="py-12 border-2 border-dashed rounded-xl text-center text-zinc-500">
        [Sprint Board Placeholder - Phase 25.6]
      </div>
    </div>
  );
}