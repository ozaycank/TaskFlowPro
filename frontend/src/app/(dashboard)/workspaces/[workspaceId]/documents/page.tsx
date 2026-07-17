'use client';

import { useState } from 'react';
import { useParams } from 'next/navigation';
import { DocumentTree } from '@/features/documents/components/DocumentTree';
import { DocumentEditor } from '@/features/documents/components/DocumentEditor';

export default function WorkspaceDocumentsPage() {
    const params = useParams();
    const workspaceId = params.workspaceId as string;

    // Local state to manage which document is currently being viewed/edited
    const [activeDocumentId, setActiveDocumentId] = useState<string | null>(null);

    return (
        <div className="flex h-full -m-8 border rounded-xl overflow-hidden border-zinc-200 dark:border-zinc-800 shadow-sm">
            {/* Left Sidebar: Document Navigation Tree */}
            <DocumentTree 
                workspaceId={workspaceId}
                activeDocumentId={activeDocumentId}
                onSelectDocument={(id) => setActiveDocumentId(id)}
                onCreateDocument={() => setActiveDocumentId(null)}
            />

            {/* Right Area: Document Editor */}
            <DocumentEditor 
                workspaceId={workspaceId}
                documentId={activeDocumentId}
                onDocumentDeleted={() => setActiveDocumentId(null)}
                onDocumentCreated={(newId) => setActiveDocumentId(newId)}
            />
        </div>
    );
}