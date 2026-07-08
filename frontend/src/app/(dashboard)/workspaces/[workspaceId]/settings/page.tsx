'use client';

import { useParams } from 'next/navigation';
import { useWorkspaceDetailQuery } from '@/features/workspace/hooks/useWorkspaceSettings';
import { WorkspaceSettingsForm } from '@/features/workspace/components/WorkspaceSettingsForm';
import { DeleteWorkspaceZone } from '@/features/workspace/components/DeleteWorkspaceZone';
import { Skeleton } from '@/components/ui/skeleton';
import { WorkspaceCustomFieldsSettings } from '@/features/custom-fields/components/WorkspaceCustomFieldsSettings';

export default function WorkspaceSettingsPage() {
    const params = useParams();
    const workspaceId = params.workspaceId as string;

    const { data: workspace, isLoading, isError } = useWorkspaceDetailQuery(workspaceId);

    if (isLoading) {
        return (
            <div className="max-w-4xl mx-auto space-y-8 p-6">
                <Skeleton className="h-10 w-1/3" />
                <Skeleton className="h-[300px] w-full" />
                <Skeleton className="h-[200px] w-full" />
            </div>
        );
    }

    if (isError || !workspace) {
        return <div className="p-6 text-red-500">Failed to load workspace settings.</div>;
    }

    return (
        <div className="max-w-4xl mx-auto pb-12 w-full">
            <div className="mb-8">
                <h1 className="text-3xl font-bold tracking-tight text-zinc-900 dark:text-zinc-50">Workspace Settings</h1>
                <p className="text-sm text-zinc-500 mt-2">Manage your workspace preferences and administrative controls.</p>
            </div>

            <div className="bg-white dark:bg-zinc-900 border border-zinc-200 dark:border-zinc-800 rounded-xl p-6 shadow-sm">
                <h2 className="text-xl font-semibold mb-6 dark:text-zinc-100">General Information</h2>
                <WorkspaceSettingsForm workspace={workspace} />
                <WorkspaceCustomFieldsSettings workspaceId={workspaceId} />
            </div>

            {/* In a real app, you might want to conditionally render this only if the user is the Owner */}
            <DeleteWorkspaceZone workspace={workspace} />
        </div>
    );
}