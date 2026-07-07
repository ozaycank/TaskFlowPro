'use client';

import { useWorklogsQuery, useDeleteWorklogMutation } from '../hooks/useTimeTrackingHooks';
import { Skeleton } from '@/components/ui/skeleton';
import { format } from 'date-fns';
import { Clock, Trash2 } from 'lucide-react';

const formatSeconds = (seconds: number) => {
    const hours = Math.floor(seconds / 3600);
    const minutes = Math.floor((seconds % 3600) / 60);
    if (hours > 0 && minutes > 0) return `${hours}h ${minutes}m`;
    if (hours > 0) return `${hours}h`;
    return `${minutes}m`;
};

export const WorklogList = ({ taskId }: { taskId: string }) => {
    const { data: worklogs, isLoading } = useWorklogsQuery(taskId);
    const { mutate: deleteWorklog, isPending: isDeleting } = useDeleteWorklogMutation(taskId);

    if (isLoading) return <Skeleton className="w-full h-32 rounded-xl" />;

    if (!worklogs || worklogs.length === 0) {
        return <div className="text-sm text-zinc-500 italic py-4">No time logged yet.</div>;
    }

    const totalSeconds = worklogs.reduce((acc, log) => acc + log.timeSpentSeconds, 0);

    return (
        <div className="space-y-4">
            <div className="flex items-center gap-2 text-sm font-medium text-zinc-700 dark:text-zinc-300 bg-zinc-50 dark:bg-zinc-800/50 p-3 rounded-lg border border-zinc-200 dark:border-zinc-800">
                <Clock size={16} className="text-indigo-500" />
                <span>Total Time Spent:</span>
                <span className="font-bold text-indigo-600 dark:text-indigo-400">{formatSeconds(totalSeconds)}</span>
            </div>

            <div className="space-y-2">
                {worklogs.map(log => (
                    <div key={log.id} className="flex items-start justify-between p-3 border border-zinc-200 dark:border-zinc-800 rounded-lg hover:bg-zinc-50 dark:hover:bg-zinc-900/50 transition-colors group">
                        <div className="flex gap-3">
                            <div className="w-8 h-8 rounded-full bg-blue-100 dark:bg-blue-900/50 flex items-center justify-center font-bold text-blue-700 dark:text-blue-400 text-xs">
                                U
                            </div>
                            <div>
                                <div className="flex items-center gap-2">
                                    <span className="text-sm font-semibold dark:text-zinc-200">User</span>
                                    <span className="text-xs text-zinc-500">
                                        {format(new Date(log.startDate), 'MMM d, yyyy')}
                                    </span>
                                </div>
                                {log.description && (
                                    <p className="text-xs text-zinc-600 dark:text-zinc-400 mt-1">{log.description}</p>
                                )}
                            </div>
                        </div>
                        <div className="flex items-center gap-4">
                            <span className="text-sm font-semibold bg-zinc-100 dark:bg-zinc-800 px-2 py-1 rounded">
                                {formatSeconds(log.timeSpentSeconds)}
                            </span>
                            <button 
                                onClick={() => deleteWorklog(log.id)}
                                disabled={isDeleting}
                                className="text-zinc-400 hover:text-red-500 opacity-0 group-hover:opacity-100 transition-opacity"
                                title="Delete worklog"
                            >
                                <Trash2 size={16} />
                            </button>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};