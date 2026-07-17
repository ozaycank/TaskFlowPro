'use client';

import { useDocumentTreeQuery } from '../hooks/useDocumentQueries';
import { FileText, Plus, File } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Skeleton } from '@/components/ui/skeleton';

interface DocumentTreeProps {
    workspaceId: string;
    projectId?: string | null;
    activeDocumentId?: string | null;
    onSelectDocument: (id: string) => void;
    onCreateDocument: () => void;
}

export const DocumentTree = ({ workspaceId, projectId, activeDocumentId, onSelectDocument, onCreateDocument }: DocumentTreeProps) => {
    const { data: documents, isLoading, isError } = useDocumentTreeQuery(workspaceId, projectId);

    if (isLoading) {
        return (
            <div className="space-y-2 p-4">
                <Skeleton className="h-4 w-3/4" />
                <Skeleton className="h-4 w-1/2" />
                <Skeleton className="h-4 w-5/6" />
            </div>
        );
    }

    if (isError) {
        return <div className="p-4 text-sm text-red-500">Failed to load documents.</div>;
    }

    return (
        <div className="flex flex-col h-full border-r border-zinc-200 dark:border-zinc-800 bg-zinc-50 dark:bg-zinc-950 w-64 flex-shrink-0">
            <div className="p-4 border-b border-zinc-200 dark:border-zinc-800 flex justify-between items-center">
                <h3 className="font-semibold text-sm flex items-center gap-2">
                    <FileText size={16} />
                    Documents
                </h3>
                <Button variant="ghost" size="icon" className="h-6 w-6" onClick={onCreateDocument}>
                    <Plus size={14} />
                </Button>
            </div>
            
            <div className="flex-1 overflow-y-auto p-2 space-y-1">
                {!documents || documents.length === 0 ? (
                    <div className="text-sm text-zinc-500 text-center p-4">No documents yet.</div>
                ) : (
                    documents.map((doc) => (
                        <button
                            key={doc.id}
                            onClick={() => onSelectDocument(doc.id)}
                            className={`w-full text-left flex items-center gap-2 px-3 py-2 rounded-md text-sm transition-colors ${
                                activeDocumentId === doc.id
                                    ? 'bg-zinc-200 dark:bg-zinc-800 font-medium text-zinc-900 dark:text-white'
                                    : 'text-zinc-600 dark:text-zinc-400 hover:bg-zinc-100 dark:hover:bg-zinc-800/50 hover:text-zinc-900 dark:hover:text-white'
                            }`}
                        >
                            <span>{doc.emojiIcon || <File size={14} />}</span>
                            <span className="truncate">{doc.title}</span>
                        </button>
                    ))
                )}
            </div>
        </div>
    );
};