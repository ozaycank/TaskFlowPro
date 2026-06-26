'use client';

import { useAttachmentsQuery } from '../hooks/useCollaborationHooks';
import { Skeleton } from '@/components/ui/skeleton';
import { FileIcon } from 'lucide-react';
import { formatDistanceToNow } from 'date-fns';

export const AttachmentList = ({ taskId }: { taskId: string }) => {
    const { data: attachments, isLoading } = useAttachmentsQuery(taskId);

    if (isLoading) return <Skeleton className="w-full h-32 rounded-xl" />;
    
    if (!attachments || attachments.length === 0) {
        return null; // Don't show anything if there are no attachments
    }

    return (
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4 mt-4">
            {attachments.map(att => (
                <div key={att.id} className="flex items-center gap-3 p-3 border border-zinc-200 dark:border-zinc-800 rounded-xl bg-white dark:bg-zinc-950 hover:bg-zinc-50 dark:hover:bg-zinc-900 transition-colors cursor-pointer">
                    <div className="p-2 bg-indigo-50 dark:bg-indigo-900/30 rounded-lg text-indigo-600 dark:text-indigo-400">
                        <FileIcon size={16} />
                    </div>
                    <div className="flex-1 overflow-hidden">
                        <p className="text-sm font-medium truncate dark:text-zinc-200">{att.fileName}</p>
                        <p className="text-xs text-zinc-500">{(att.fileSizeBytes / 1024).toFixed(1)} KB • {formatDistanceToNow(new Date(att.createdAt))} ago</p>
                    </div>
                </div>
            ))}
        </div>
    );
};