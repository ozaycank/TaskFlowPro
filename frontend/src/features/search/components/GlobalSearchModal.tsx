'use client';

import { useState, useEffect, useRef } from 'react';
import { useRouter } from 'next/navigation';
import { useWorkspaceStore } from '@/features/workspace/stores/useWorkspaceStore';
import { useGlobalSearchQuery } from '../hooks/useSearchQueries';
import { Dialog, DialogContent, DialogTitle } from '@/components/ui/dialog';
import { Input } from '@/components/ui/input';
import { Search, FileText, CheckSquare, FolderKanban, Loader2, Command } from 'lucide-react';

export const GlobalSearchModal = () => {
    const [open, setOpen] = useState(false);
    const [searchTerm, setSearchTerm] = useState('');
    const [debouncedTerm, setDebouncedTerm] = useState('');
    
    const router = useRouter();
    const activeWorkspaceId = useWorkspaceStore(state => state.activeWorkspaceId);

    // Debounce logic to prevent API spam while typing
    useEffect(() => {
        const timer = setTimeout(() => {
            setDebouncedTerm(searchTerm);
        }, 300); // 300ms delay

        return () => clearTimeout(timer);
    }, [searchTerm]);

    // Handle Keyboard Shortcut (Cmd+K / Ctrl+K)
    useEffect(() => {
        const down = (e: KeyboardEvent) => {
            if (e.key === 'k' && (e.metaKey || e.ctrlKey)) {
                e.preventDefault();
                setOpen((open) => !open);
            }
        };

        document.addEventListener('keydown', down);
        return () => document.removeEventListener('keydown', down);
    }, []);

    const { data: results, isLoading, isError } = useGlobalSearchQuery(
        activeWorkspaceId || '', 
        debouncedTerm
    );

    const handleSelectResult = (url: string) => {
        setOpen(false);
        router.push(url);
    };

    // Helper to get correct icon based on entity type
    const getIcon = (type: string) => {
        switch (type.toLowerCase()) {
            case 'task': return <CheckSquare className="text-blue-500" size={16} />;
            case 'document': return <FileText className="text-orange-500" size={16} />;
            case 'project': return <FolderKanban className="text-purple-500" size={16} />;
            default: return <Search className="text-zinc-500" size={16} />;
        }
    };

    return (
        <>
            {/* The Trigger Button (Optional visual hint) */}
            <button 
                onClick={() => setOpen(true)}
                className="hidden md:flex items-center gap-2 px-3 py-1.5 text-sm text-zinc-500 dark:text-zinc-400 bg-zinc-100 dark:bg-zinc-900 border border-zinc-200 dark:border-zinc-800 rounded-md hover:bg-zinc-200 dark:hover:bg-zinc-800 transition-colors w-64"
            >
                <Search size={14} />
                <span className="flex-1 text-left">Search workspace...</span>
                <kbd className="hidden lg:inline-flex items-center gap-1 font-mono text-[10px] font-medium opacity-100">
                    <span className="text-xs">⌘</span>K
                </kbd>
            </button>

            <Dialog open={open} onOpenChange={setOpen}>
                <DialogContent className="p-0 sm:max-w-2xl gap-0 overflow-hidden shadow-2xl rounded-xl">
                    <DialogTitle className="sr-only">Global Search</DialogTitle>
                    
                    <div className="flex items-center border-b border-zinc-200 dark:border-zinc-800 px-3">
                        <Search className="mr-2 h-4 w-4 shrink-0 text-zinc-500" />
                        <Input 
                            value={searchTerm}
                            onChange={(e) => setSearchTerm(e.target.value)}
                            placeholder="Type a command or search..." 
                            className="flex h-12 w-full rounded-md bg-transparent py-3 text-sm outline-none border-0 focus-visible:ring-0 shadow-none"
                            autoFocus
                        />
                        {isLoading && debouncedTerm && <Loader2 className="ml-2 h-4 w-4 animate-spin text-zinc-500" />}
                    </div>

                    <div className="max-h-[60vh] overflow-y-auto p-2">
                        {!debouncedTerm && (
                            <div className="py-6 text-center text-sm text-zinc-500 flex flex-col items-center">
                                <Command className="h-8 w-8 mb-2 opacity-20" />
                                Search for tasks, documents, or projects.
                            </div>
                        )}

                        {debouncedTerm && results?.length === 0 && !isLoading && (
                            <div className="py-6 text-center text-sm text-zinc-500">
                                No results found for "{debouncedTerm}".
                            </div>
                        )}

                        {isError && (
                            <div className="py-6 text-center text-sm text-red-500">
                                An error occurred while searching.
                            </div>
                        )}

                        <div className="space-y-1">
                            {results?.map((result, idx) => (
                                <button
                                    key={idx}
                                    onClick={() => handleSelectResult(result.url)}
                                    className="w-full flex items-start gap-3 p-3 rounded-lg hover:bg-zinc-100 dark:hover:bg-zinc-800/50 transition-colors text-left group"
                                >
                                    <div className="mt-0.5 bg-white dark:bg-zinc-900 p-1.5 rounded-md border border-zinc-200 dark:border-zinc-800 shadow-sm group-hover:shadow-md transition-shadow">
                                        {getIcon(result.entityType)}
                                    </div>
                                    <div className="flex-1 min-w-0">
                                        <p className="text-sm font-medium text-zinc-900 dark:text-zinc-100 truncate">
                                            {result.title}
                                        </p>
                                        <p className="text-xs text-zinc-500 truncate mt-0.5">
                                            {result.snippet}
                                        </p>
                                    </div>
                                    <div className="text-[10px] uppercase tracking-wider font-semibold text-zinc-400 bg-zinc-100 dark:bg-zinc-900 px-2 py-1 rounded">
                                        {result.entityType}
                                    </div>
                                </button>
                            ))}
                        </div>
                    </div>
                </DialogContent>
            </Dialog>
        </>
    );
};