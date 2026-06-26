'use client';

import { useCommentsQuery } from '../hooks/useCollaborationHooks';
import { Skeleton } from '@/components/ui/skeleton';
import { formatDistanceToNow } from 'date-fns';

export const CommentList = ({ taskId }: { taskId: string }) => {
    const { data: comments, isLoading } = useCommentsQuery(taskId);

    if (isLoading) return <Skeleton className="w-full h-32 rounded-xl" />;
    
    if (!comments || comments.length === 0) {
        return <div className="text-sm text-zinc-500 italic py-4">No comments yet. Start the conversation!</div>;
    }

    return (
        <div className="space-y-6">
            {comments.map(comment => (
                <div key={comment.id} className="flex gap-4">
                    <div className="w-8 h-8 rounded-full bg-indigo-100 dark:bg-indigo-900/50 flex-shrink-0 flex items-center justify-center font-bold text-indigo-700 dark:text-indigo-400 text-xs">
                        U
                    </div>
                    <div className="flex-1 space-y-1">
                        <div className="flex items-center gap-2">
                            <span className="text-sm font-semibold dark:text-zinc-200">User</span>
                            <span className="text-xs text-zinc-500">
                                {formatDistanceToNow(new Date(comment.createdAt), { addSuffix: true })}
                            </span>
                        </div>
                        <div className="text-sm text-zinc-700 dark:text-zinc-300 whitespace-pre-wrap bg-zinc-50 dark:bg-zinc-900/50 p-3 rounded-xl border border-zinc-100 dark:border-zinc-800/50">
                            {comment.content}
                        </div>
                    </div>
                </div>
            ))}
        </div>
    );
};