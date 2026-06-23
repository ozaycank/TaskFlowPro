import { ProjectDto } from '../types/project.types';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import Link from 'next/link';

export const ProjectCard = ({ project }: { project: ProjectDto }) => {
  return (
    <Link href={`/projects/${project.id}`}>
      <Card className="hover:shadow-md transition-shadow cursor-pointer h-full border-zinc-200 dark:border-zinc-800">
        <CardHeader className="flex flex-row items-center justify-between pb-2">
          <CardTitle className="text-lg font-semibold flex items-center gap-2">
            <div className="w-3 h-3 rounded-full" style={{ backgroundColor: project.color }} />
            {project.name}
          </CardTitle>
          {project.isArchived && <Badge variant="secondary">Archived</Badge>}
        </CardHeader>
        <CardContent>
          <p className="text-sm text-zinc-500 dark:text-zinc-400 line-clamp-2">
            {project.description || 'No description provided.'}
          </p>
        </CardContent>
      </Card>
    </Link>
  );
};