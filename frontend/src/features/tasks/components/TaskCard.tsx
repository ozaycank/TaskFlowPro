'use client';

import { Draggable } from '@hello-pangea/dnd';
import { TaskDto, PriorityLevel } from '../../tasks/types/task.types';
import { format } from 'date-fns';
import Link from 'next/link';

interface TaskCardProps {
    task: TaskDto;
    index: number;
    workspaceId: string;
    projectId: string;
}

const getPriorityColor = (priority: PriorityLevel) => {
    switch (priority) {
        case PriorityLevel.Low: return 'bg-blue-100 text-blue-700 dark:bg-blue-900/30 dark:text-blue-400';
        case PriorityLevel.Medium: return 'bg-green-100 text-green-700 dark:bg-green-900/30 dark:text-green-400';
        case PriorityLevel.High: return 'bg-orange-100 text-orange-700 dark:bg-orange-900/30 dark:text-orange-400';
        case PriorityLevel.Urgent: return 'bg-red-100 text-red-700 dark:bg-red-900/30 dark:text-red-400';
        case PriorityLevel.Critical: return 'bg-purple-100 text-purple-700 dark:bg-purple-900/30 dark:text-purple-400';
        default: return 'bg-zinc-100 text-zinc-700 dark:bg-zinc-800 dark:text-zinc-400';
    }
};

// YENİ: Etiketler için metin tabanlı tutarlı bir renk üretici
const getLabelColor = (label: string) => {
    const colors = [
        'bg-emerald-100 text-emerald-700 dark:bg-emerald-900/30 dark:text-emerald-400',
        'bg-amber-100 text-amber-700 dark:bg-amber-900/30 dark:text-amber-400',
        'bg-fuchsia-100 text-fuchsia-700 dark:bg-fuchsia-900/30 dark:text-fuchsia-400',
        'bg-cyan-100 text-cyan-700 dark:bg-cyan-900/30 dark:text-cyan-400',
        'bg-rose-100 text-rose-700 dark:bg-rose-900/30 dark:text-rose-400',
    ];
    let hash = 0;
    for (let i = 0; i < label.length; i++) hash = label.charCodeAt(i) + ((hash << 5) - hash);
    return colors[Math.abs(hash) % colors.length];
};

export const TaskCard = ({ task, index, workspaceId, projectId }: TaskCardProps) => {
    return (
        <Draggable draggableId={task.id} index={index}>
            {(provided, snapshot) => (
                <div
                    ref={provided.innerRef}
                    {...provided.draggableProps}
                    {...provided.dragHandleProps}
                    style={provided.draggableProps.style as React.CSSProperties}
                    className={`p-4 bg-white dark:bg-zinc-900 rounded-lg shadow-sm border border-zinc-200 dark:border-zinc-800 group
                        ${snapshot.isDragging ? 'shadow-lg ring-2 ring-indigo-500/50 opacity-90' : 'hover:border-zinc-300 dark:hover:border-zinc-700'}
                    `}
                >
                    <Link href={`/workspaces/${workspaceId}/projects/${projectId}/tasks/${task.id}`} className="block">
                        <div className="flex justify-between items-start mb-2">
                            <span className={`text-xs font-semibold px-2 py-1 rounded-full ${getPriorityColor(task.priority)}`}>
                                {PriorityLevel[task.priority]}
                            </span>
                            {task.assigneeId && (
                                <div className="w-6 h-6 rounded-full bg-zinc-200 dark:bg-zinc-700 flex items-center justify-center text-[10px] font-bold">
                                    A
                                </div>
                            )}
                        </div>
                        
                        <h4 className="text-sm font-medium text-zinc-900 dark:text-zinc-100 mb-1 leading-tight">
                            {task.title}
                        </h4>

                        {task.labels && task.labels.length > 0 && (
                            <div className="flex flex-wrap gap-1 mt-2 mb-2">
                                {task.labels.map(label => (
                                    <span key={label} className={`text-[10px] px-2 py-0.5 rounded-md font-medium ${getLabelColor(label)}`}>
                                        {label}
                                    </span>
                                ))}
                            </div>
                        )}
                        
                        {task.description && (
                            <p className="text-xs text-zinc-500 dark:text-zinc-400 line-clamp-2 mb-3 mt-1">
                                {task.description}
                            </p>
                        )}

                        <div className="flex items-center text-[10px] text-zinc-400 mt-2">
                            <span>{format(new Date(task.createdAt), 'MMM d, yyyy')}</span>
                        </div>
                    </Link>
                </div>
            )}
        </Draggable>
    );
};