'use client';

import { TaskBoard } from '@/features/tasks/components/TaskBoard';

export default function ProjectDetailsPage() {
    return (
        <div className="flex flex-col h-[calc(100vh-2rem)] w-full">
            <div className="mb-6 flex-shrink-0">
                <h1 className="text-3xl font-bold tracking-tight text-zinc-900 dark:text-zinc-50">Sprint Board</h1>
                <p className="text-zinc-500">Manage tasks and workflows for this project.</p>
            </div>
            
            {/* The Kanban Board takes up the remaining vertical space */}
            <div className="flex-1 overflow-hidden min-h-0">
                <TaskBoard />
            </div>
        </div>
    );
}