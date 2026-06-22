'use client';

import { useWorkspacesQuery } from '../hooks/useWorkspacesQuery';
import { useWorkspaceStore } from '../stores/useWorkspaceStore';
import { 
  DropdownMenu, 
  DropdownMenuContent, 
  DropdownMenuItem, 
  DropdownMenuLabel, 
  DropdownMenuSeparator, 
  DropdownMenuTrigger 
} from '@/components/ui/dropdown-menu';
import { Button } from '@/components/ui/button';
import { ChevronsUpDown, Building2 } from 'lucide-react';
import { CreateWorkspaceDialog } from './CreateWorkspaceDialog';
import { useRouter } from 'next/navigation';

export const WorkspaceSwitcher = () => {
  const { data: workspaces, isLoading } = useWorkspacesQuery();
  const { activeWorkspaceId, setActiveWorkspace } = useWorkspaceStore();
  const router = useRouter();

  if (isLoading) return <div className="h-10 bg-zinc-100 dark:bg-zinc-900 animate-pulse rounded-md w-full" />;

  const activeWorkspace = workspaces?.find((w) => w.id === activeWorkspaceId) || workspaces?.[0];

  // Fallback if no workspaces exist
  if (!workspaces || workspaces.length === 0) {
    return <CreateWorkspaceDialog />;
  }

  // Ensure active workspace is set if not present but workspaces exist
  if (!activeWorkspaceId && activeWorkspace) {
    setActiveWorkspace(activeWorkspace.id);
  }

  const handleSwitch = (id: string) => {
    setActiveWorkspace(id);
    router.push(`/workspaces/${id}`); // Route to specific workspace dashboard
  };

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button variant="ghost" className="w-full justify-between h-12 px-3 hover:bg-zinc-100 dark:hover:bg-zinc-800">
          <div className="flex items-center gap-2 truncate">
            <div className="flex h-8 w-8 items-center justify-center rounded-md bg-zinc-900 text-white dark:bg-white dark:text-zinc-900">
              <Building2 size={16} />
            </div>
            <span className="truncate font-medium">{activeWorkspace?.name || 'Select Workspace'}</span>
          </div>
          <ChevronsUpDown size={16} className="text-zinc-500" />
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent className="w-56" align="start">
        <DropdownMenuLabel>Your Workspaces</DropdownMenuLabel>
        <DropdownMenuSeparator />
        {workspaces.map((ws) => (
          <DropdownMenuItem 
            key={ws.id} 
            onClick={() => handleSwitch(ws.id)}
            className={`cursor-pointer ${activeWorkspaceId === ws.id ? 'bg-zinc-100 dark:bg-zinc-800' : ''}`}
          >
            {ws.name}
          </DropdownMenuItem>
        ))}
        <DropdownMenuSeparator />
        <CreateWorkspaceDialog />
      </DropdownMenuContent>
    </DropdownMenu>
  );
};