import { PlusCircle } from 'lucide-react';
import { CreateWorkspaceDialog } from './CreateWorkspaceDialog';
import { Button } from '@/components/ui/button';

// PHASE 4 FIX: Removed onAction prop, handling dialog internally in a React way
export const WorkspaceEmptyState = () => {
  return (
    <div className="flex flex-col items-center justify-center p-12 text-center border-2 border-dashed border-zinc-200 dark:border-zinc-800 rounded-xl">
      <div className="w-12 h-12 bg-zinc-100 dark:bg-zinc-900 rounded-full flex items-center justify-center mb-4">
        <PlusCircle className="text-zinc-500" size={24} />
      </div>
      <h3 className="text-lg font-semibold dark:text-white mb-1">No Workspaces Found</h3>
      <p className="text-zinc-500 mb-6 max-w-sm">You don't belong to any workspace yet. Create one to get started with your team.</p>
      
      <CreateWorkspaceDialog 
        trigger={<Button>Create Workspace</Button>} 
      />
    </div>
  );
};