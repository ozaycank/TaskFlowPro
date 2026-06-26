'use client';

import { useState } from 'react';
import { Button } from '@/components/ui/button';
import { useCreateCommentMutation } from '../hooks/useCollaborationHooks';

export const CommentEditor = ({ taskId }: { taskId: string }) => {
    const [content, setContent] = useState('');
    const { mutate: createComment, isPending } = useCreateCommentMutation(taskId);

    const handleSubmit = () => {
        if (!content.trim()) return;
        createComment({ taskId, content }, {
            onSuccess: () => setContent('') // Clear after success
        });
    };

    return (
        <div className="border border-zinc-200 dark:border-zinc-800 rounded-xl overflow-hidden bg-white dark:bg-zinc-950 focus-within:ring-2 focus-within:ring-indigo-500 transition-shadow">
            <textarea
                value={content}
                onChange={(e) => setContent(e.target.value)}
                placeholder="Add a comment..."
                className="w-full min-h-[100px] p-3 resize-none bg-transparent outline-none text-sm dark:text-zinc-100"
            />
            <div className="bg-zinc-50 dark:bg-zinc-900 border-t border-zinc-200 dark:border-zinc-800 p-2 flex justify-end">
                <Button 
                    size="sm" 
                    onClick={handleSubmit} 
                    disabled={isPending || !content.trim()}
                >
                    {isPending ? 'Sending...' : 'Comment'}
                </Button>
            </div>
        </div>
    );
};