'use client';

import { useParams } from 'next/navigation';
import { SprintBoard } from '@/features/sprints/components/SprintBoard';

export default function SprintsPage() {
    const params = useParams();
    const workspaceId = params.workspaceId as string;
    const projectId = params.projectId as string;

    return (
        <div className="h-full w-full">
            <SprintBoard workspaceId={workspaceId} projectId={projectId} />
        </div>
    );
}