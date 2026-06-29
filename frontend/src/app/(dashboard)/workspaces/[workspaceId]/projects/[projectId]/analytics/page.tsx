'use client';

import { useParams } from 'next/navigation';
import { AnalyticsDashboard } from '@/features/analytics/components/AnalyticsDashboard';

export default function ProjectAnalyticsPage() {
    const params = useParams();
    const projectId = params.projectId as string;

    return (
        <div className="h-full w-full p-2">
            <AnalyticsDashboard projectId={projectId} />
        </div>
    );
}