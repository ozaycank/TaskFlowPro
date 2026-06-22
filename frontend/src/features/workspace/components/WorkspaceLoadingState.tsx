import { Skeleton } from '@/components/ui/skeleton';

export const WorkspaceLoadingState = () => {
  return (
    <div className="space-y-4 p-6">
      <Skeleton className="h-8 w-[200px]" />
      <Skeleton className="h-[400px] w-full rounded-xl" />
    </div>
  );
};