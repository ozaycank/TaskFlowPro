'use client';

import { formatDistanceToNow } from 'date-fns';
import { useActivityFeedQuery } from '../hooks/useActivityHooks';
import { ActivityQueryParams } from '../types/activity.types';
import { getActivityIcon } from '../constants/activity.constants';
import { Skeleton } from '@/components/ui/skeleton';
import { Activity } from 'lucide-react';

interface Props {
    params: ActivityQueryParams;
    title?: string;
}

export const ActivityTimeline = ({ params, title = "Audit Log" }: Props) => {
    const { data: logs, isLoading, isError } = useActivityFeedQuery(params);

    if (isLoading) {
        return (
            <div className="space-y-4">
                <Skeleton className="h-12 w-full" />
                <Skeleton className="h-12 w-full" />
                <Skeleton className="h-12 w-full" />
            </div>
        );
    }

    if (isError || !logs) {
        return <div className="text-sm text-red-500 p-4 border border-red-200 rounded-lg">Failed to load activity feed.</div>;
    }

    if (logs.length === 0) {
        return (
            <div className="flex flex-col items-center justify-center p-8 text-zinc-500 border border-dashed border-zinc-200 dark:border-zinc-800 rounded-lg">
                <Activity size={32} className="mb-2 opacity-20" />
                <p className="text-sm">No recent activity found.</p>
            </div>
        );
    }

    return (
        <div className="bg-white dark:bg-zinc-950 border border-zinc-200 dark:border-zinc-800 rounded-xl overflow-hidden shadow-sm">
            <div className="border-b border-zinc-200 dark:border-zinc-800 px-6 py-4">
                <h3 className="font-semibold text-lg dark:text-zinc-100">{title}</h3>
            </div>
            <div className="p-6">
                <div className="relative border-l border-zinc-200 dark:border-zinc-800 ml-3 space-y-6">
                    {logs.map((log) => (
                        <div key={log.id} className="relative pl-6">
                            {/* Timeline Node */}
                            <span className="absolute -left-[13px] bg-white dark:bg-zinc-950 p-1 rounded-full border border-zinc-200 dark:border-zinc-800">
                                {getActivityIcon(log.entityType, log.action)}
                            </span>
                            
                            <div className="flex flex-col sm:flex-row sm:items-baseline justify-between gap-2">
                                <div>
                                    <p className="text-sm text-zinc-800 dark:text-zinc-200">
                                        <span className="font-semibold text-indigo-600 dark:text-indigo-400">System User</span> 
                                        {' '}
                                        <span className="text-zinc-500 dark:text-zinc-400">
                                            {log.action.replace('.', ' ')} 
                                        </span>
                                    </p>
                                    {log.details && (
                                        <p className="text-xs text-zinc-500 mt-1.5 p-2 bg-zinc-50 dark:bg-zinc-900 rounded-md border border-zinc-100 dark:border-zinc-800">
                                            {log.details}
                                        </p>
                                    )}
                                </div>
                                <span className="text-xs text-zinc-400 whitespace-nowrap">
                                    {formatDistanceToNow(new Date(log.createdAt), { addSuffix: true })}
                                </span>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
};