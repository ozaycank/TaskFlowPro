'use client';

import { useParams } from 'next/navigation';
import { WorkspaceMembersTable } from '@/features/workspace/components/WorkspaceMembersTable';
import { InviteMemberDialog } from '@/features/workspace/components/InviteMemberDialog';

export default function WorkspaceMembersPage() {
  const params = useParams();
  const workspaceId = params.workspaceId as string;

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h2 className="text-2xl font-bold tracking-tight">Workspace Members</h2>
          <p className="text-zinc-500">Manage your team and their roles.</p>
        </div>
        <InviteMemberDialog workspaceId={workspaceId} />
      </div>

      <WorkspaceMembersTable workspaceId={workspaceId} />
    </div>
  );
}