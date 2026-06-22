import { Button } from '@/components/ui/button';

export const WorkspaceErrorState = ({ retry }: { retry: () => void }) => {
  return (
    <div className="flex flex-col items-center justify-center min-h-[400px] text-center">
      <h3 className="text-xl font-bold dark:text-white mb-2">Something went wrong</h3>
      <p className="text-zinc-500 mb-6">We couldn't load the workspace data.</p>
      <Button onClick={retry} variant="outline">Try Again</Button>
    </div>
  );
};