import { useEffect } from 'react';
import { useWorkspaceStore } from '../stores/useWorkspaceStore';
import { useWorkspacesQuery } from '../hooks/useWorkspacesQuery';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { useRouter, useParams } from 'next/navigation';

export const WorkspaceSwitcher = () => {
    const { data: workspaces, isLoading } = useWorkspacesQuery();
    const activeWorkspaceId = useWorkspaceStore((state) => state.activeWorkspaceId);
    const setActiveWorkspace = useWorkspaceStore((state) => state.setActiveWorkspace);
    const router = useRouter();
    const params = useParams();

    useEffect(() => {
        // ERROR FIXED: Only update state inside useEffect to prevent "setState during render" error
        if (!isLoading && workspaces && workspaces.length > 0) {
            const urlWorkspaceId = params.workspaceId as string;
            
            // If URL has an ID, make sure store is synced
            if (urlWorkspaceId && urlWorkspaceId !== activeWorkspaceId) {
                setActiveWorkspace(urlWorkspaceId);
            } 
            // If no active workspace and no URL ID, select the first one
            else if (!activeWorkspaceId && !urlWorkspaceId) {
                setActiveWorkspace(workspaces[0].id);
                router.push(`/workspaces/${workspaces[0].id}`);
            }
        }
    }, [isLoading, workspaces, activeWorkspaceId, params.workspaceId, setActiveWorkspace, router]);

    if (isLoading) return <div className="h-10 w-full bg-zinc-100 dark:bg-zinc-800 animate-pulse rounded-md" />;
    if (!workspaces || workspaces.length === 0) return null;

    const handleValueChange = (value: string) => {
        setActiveWorkspace(value);
        router.push(`/workspaces/${value}`);
    };

    return (
        <Select value={activeWorkspaceId || undefined} onValueChange={handleValueChange}>
            <SelectTrigger className="w-full font-semibold border-0 bg-transparent hover:bg-zinc-100 dark:hover:bg-zinc-800 focus:ring-0">
                <SelectValue placeholder="Select Workspace" />
            </SelectTrigger>
            <SelectContent>
                {workspaces.map((ws) => (
                    <SelectItem key={ws.id} value={ws.id}>
                        {ws.name}
                    </SelectItem>
                ))}
            </SelectContent>
        </Select>
    );
};