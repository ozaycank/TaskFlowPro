'use client';

import { useState, useEffect } from 'react';
import { useDocumentDetailQuery } from '../hooks/useDocumentQueries';
import { useCreateDocumentMutation, useUpdateDocumentMutation, useDeleteDocumentMutation } from '../hooks/useDocumentMutations';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Skeleton } from '@/components/ui/skeleton';
import { Trash2, Save } from 'lucide-react';
import { useEditor, EditorContent } from '@tiptap/react';
import StarterKit from '@tiptap/starter-kit';
import Placeholder from '@tiptap/extension-placeholder';

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
    const [emoji, setEmoji] = useState('📄');
    const [error, setError] = useState<string | null>(null);

    const editor = useEditor({
        extensions: [
            StarterKit,
            Placeholder.configure({
                placeholder: 'Start writing your documentation here... (Type / for commands)',
                emptyEditorClass: 'is-editor-empty',
            }),
        ],
        content: '', 
        editorProps: {
            attributes: {
                class: 'prose prose-zinc dark:prose-invert max-w-none focus:outline-none min-h-[500px]',
            },
        },
    });

    // Veri yüklendiğinde editörü ve state'leri güncelle
    useEffect(() => {
        if (document && documentId) {
            setTitle(document.title || '');
            setEmoji(document.emojiIcon || '📄');
            setError(null);
            
            if (editor && document.content !== editor.getHTML()) {
                editor.commands.setContent(document.content || '');
            }
        } else if (!documentId) {
            setTitle('');
            setEmoji('📄');
            setError(null);
            if (editor) {
                editor.commands.setContent('');
            }
        }
    }, [document, documentId, editor]);

    const handleSave = async () => {
        if (!title.trim() || !editor) return;
        setError(null);

        const htmlContent = editor.getHTML();

        try {
            if (documentId) {
                // Update
                await updateMutation.mutateAsync({
                    documentId,
                    title,
                    content: htmlContent,
                    emojiIcon: emoji
                });
            } else {
                // Create
                const newId = await createMutation.mutateAsync({
                    workspaceId,
                    projectId,
                    title,
                    content: htmlContent, // YENİ: textarea yerine HTML gönderiyoruz
                    emojiIcon: emoji,
                    orderIndex: 0
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
        <div className="flex-1 flex flex-col h-full bg-white dark:bg-zinc-950 relative overflow-hidden">
            {/* Toolbar */}
            <div className="flex items-center justify-between p-4 border-b border-zinc-200 dark:border-zinc-800 bg-white dark:bg-zinc-950 z-10">
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
                    {editor && (
                        <div className="flex items-center gap-1 border-r border-zinc-200 dark:border-zinc-800 pr-4 mr-2">
                            <Button 
                                variant="ghost" size="sm" 
                                onClick={() => editor.chain().focus().toggleBold().run()}
                                className={editor.isActive('bold') ? 'bg-zinc-200 dark:bg-zinc-800' : ''}
                            >
                                <b>B</b>
                            </Button>
                            <Button 
                                variant="ghost" size="sm" 
                                onClick={() => editor.chain().focus().toggleItalic().run()}
                                className={editor.isActive('italic') ? 'bg-zinc-200 dark:bg-zinc-800' : ''}
                            >
                                <i>I</i>
                            </Button>
                        </div>
                    )}

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
                <div className="bg-red-50 text-red-600 p-3 text-sm border-b border-red-100 flex justify-between items-center z-10">
                    <span>{error}</span>
                    <button onClick={() => setError(null)} className="font-bold px-2">&times;</button>
                </div>
            )}

            {/* Editor Area */}
            <div className="flex-1 overflow-y-auto p-8 lg:px-24">
                <EditorContent editor={editor} className="min-h-full" />
            </div>
        </div>
    );
};