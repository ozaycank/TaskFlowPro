import { useWorkspaceQuery } from '../hooks/useWorkspaceQuery';
import { Skeleton } from '@/components/ui/skeleton';

export const WorkspaceHeader = ({ workspaceId }: { workspaceId: string }) => {
  const { data: workspace, isLoading } = useWorkspaceQuery(workspaceId);

  if (isLoading) return <Skeleton className="h-10 w-[300px] mb-6" />;

  return (
    <div className="mb-8">
      <h1 className="text-3xl font-bold tracking-tight text-zinc-900 dark:text-white">
        {workspace?.name}
      </h1>
      {workspace?.description && (
        <p className="text-zinc-500 mt-2">{workspace.description}</p>
      )}
    </div>
  );
};