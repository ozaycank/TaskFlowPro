'use client';

import { useState, useEffect } from 'react';
import { useDocumentDetailQuery } from '../hooks/useDocumentQueries';
import { useCreateDocumentMutation, useUpdateDocumentMutation, useDeleteDocumentMutation } from '../hooks/useDocumentMutations';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Skeleton } from '@/components/ui/skeleton';
import { Trash2, Save } from 'lucide-react';

interface DocumentEditorProps {
    workspaceId: string;
    projectId?: string | null;
    documentId: string | null; // null means create new
    onDocumentDeleted: () => void;
    onDocumentCreated: (id: string) => void;
}

export const DocumentEditor = ({ workspaceId, projectId, documentId, onDocumentDeleted, onDocumentCreated }: DocumentEditorProps) => {
    const { data: document, isLoading } = useDocumentDetailQuery(documentId || '');
    
    const createMutation = useCreateDocumentMutation(workspaceId, projectId);
    const updateMutation = useUpdateDocumentMutation(workspaceId, projectId);
    const deleteMutation = useDeleteDocumentMutation(workspaceId, projectId);

    const [title, setTitle] = useState('');
    const [content, setContent] = useState('');
    const [emoji, setEmoji] = useState('📄');
    const [error, setError] = useState<string | null>(null); // To show errors gracefully

    // Sync state when document loaded
    useEffect(() => {
        if (document && documentId) {
            // FIX: Ensure values are never undefined/null to avoid React 'uncontrolled' warning
            setTitle(document.title || '');
            setContent(document.content || '');
            setEmoji(document.emojiIcon || '📄');
            setError(null);
        } else if (!documentId) {
            setTitle('');
            setContent('');
            setEmoji('📄');
            setError(null);
        }
    }, [document, documentId]);

    const handleSave = async () => {
        if (!title.trim()) return;
        setError(null);

        try {
            if (documentId) {
                // Update
                await updateMutation.mutateAsync({
                    documentId,
                    title,
                    content,
                    emojiIcon: emoji
                });
            } else {
                // Create
                const newId = await createMutation.mutateAsync({
                    workspaceId,
                    projectId,
                    title,
                    content,
                    emojiIcon: emoji,
                    orderIndex: 0 // Simplification for MVP
                });
                onDocumentCreated(newId);
            }
        } catch (err: any) {
            console.error("Save failed:", err);
            setError(err.response?.data?.message || "Failed to save document. Check your connection or permissions.");
        }
    };

    const handleDelete = async () => {
        if (!documentId) return;
        setError(null);
        
        if (window.confirm("Are you sure you want to delete this document?")) {
            try {
                await deleteMutation.mutateAsync(documentId);
                onDocumentDeleted();
            } catch (err: any) {
                console.error("Delete failed:", err);
                setError(err.response?.data?.message || "Failed to delete document. You might not have permission.");
            }
        }
    };

    if (documentId && isLoading) {
        return (
            <div className="flex-1 p-8 space-y-4">
                <Skeleton className="h-12 w-1/3" />
                <Skeleton className="h-64 w-full" />
            </div>
        );
    }

    const isSaving = createMutation.isPending || updateMutation.isPending;

    return (
        <div className="flex-1 flex flex-col h-full bg-white dark:bg-zinc-950 relative">
            {/* Toolbar */}
            <div className="flex items-center justify-between p-4 border-b border-zinc-200 dark:border-zinc-800">
                <div className="flex items-center gap-2">
                    <Input 
                        value={emoji} 
                        onChange={(e) => setEmoji(e.target.value)} 
                        className="w-16 text-center text-xl" 
                        maxLength={2}
                    />
                    <Input 
                        value={title} 
                        onChange={(e) => setTitle(e.target.value)} 
                        placeholder="Document Title" 
                        className="text-lg font-semibold border-none shadow-none focus-visible:ring-0 w-96"
                    />
                </div>
                <div className="flex items-center gap-2">
                    {documentId && (
                        <Button variant="destructive" size="icon" onClick={handleDelete} disabled={deleteMutation.isPending}>
                            <Trash2 size={16} />
                        </Button>
                    )}
                    <Button onClick={handleSave} disabled={isSaving || !title.trim()}>
                        <Save size={16} className="mr-2" />
                        {isSaving ? 'Saving...' : 'Save'}
                    </Button>
                </div>
            </div>

            {/* Error Banner */}
            {error && (
                <div className="bg-red-50 text-red-600 p-3 text-sm border-b border-red-100 flex justify-between items-center">
                    <span>{error}</span>
                    <button onClick={() => setError(null)} className="font-bold px-2">&times;</button>
                </div>
            )}

            {/* Editor Area */}
            <div className="flex-1 p-6">
                <textarea
                    value={content}
                    onChange={(e) => setContent(e.target.value)}
                    placeholder="Start writing your documentation here... (Markdown supported)"
                    className="w-full h-full resize-none outline-none bg-transparent text-zinc-800 dark:text-zinc-200 leading-relaxed"
                />
            </div>
        </div>
    );
};