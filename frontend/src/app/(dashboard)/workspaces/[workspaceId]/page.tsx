'use client';

import { useParams } from 'next/navigation';
import { WorkspaceHeader } from '@/features/workspace/components/WorkspaceHeader';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';

export default function WorkspaceDashboardPage() {
  const params = useParams();
  const workspaceId = params.workspaceId as string;

  return (
    <div className="space-y-6">
      <WorkspaceHeader workspaceId={workspaceId} />
      
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        {/* Placeholder cards for future phases */}
        <Card>
          <CardHeader>
            <CardTitle className="text-lg">Active Projects</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-3xl font-bold">12</div>
          </CardContent>
        </Card>
        <Card>
          <CardHeader>
            <CardTitle className="text-lg">Tasks in Progress</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-3xl font-bold">34</div>
          </CardContent>
        </Card>
        <Card>
          <CardHeader>
            <CardTitle className="text-lg">Active Sprint</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-zinc-500">Sprint 14 • Ends in 2 days</div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}