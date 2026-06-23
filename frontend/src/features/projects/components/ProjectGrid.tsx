'use client';

import { useProjectsQuery } from '../hooks/useProjectsQuery';
import { ProjectCard } from './ProjectCard';
import { useProjectStore } from '../stores/useProjectStore';
import { Skeleton } from '@/components/ui/skeleton';

export const ProjectGrid = () => {
  const { data: projects, isLoading } = useProjectsQuery();
  const filters = useProjectStore((state) => state.projectFilters);

  if (isLoading) {
    return (
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {[1, 2, 3].map((i) => <Skeleton key={i} className="h-32 w-full rounded-xl" />)}
      </div>
    );
  }

  const filteredProjects = projects?.filter(p => 
    (filters.showArchived || !p.isArchived) &&
    p.name.toLowerCase().includes(filters.searchQuery.toLowerCase())
  );

  if (!filteredProjects?.length) {
    return <div className="text-center py-12 text-zinc-500 border rounded-lg border-dashed">No projects found.</div>;
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      {filteredProjects.map((project) => (
        <ProjectCard key={project.id} project={project} />
      ))}
    </div>
  );
};