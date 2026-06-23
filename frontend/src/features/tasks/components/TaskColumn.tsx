'use client';

import { Droppable } from '@hello-pangea/dnd';
import { WorkflowStateDto } from '@/features/workflows/types/workflow.types';
import { TaskDto } from '../../workflows/types/task.types';
import { TaskCard } from './TaskCard';

interface TaskColumnProps {
    state: WorkflowStateDto;
    tasks: TaskDto[];
}

export const TaskColumn = ({ state, tasks }: TaskColumnProps) => {
    // Sort tasks strictly by orderIndex before rendering
    const sortedTasks = [...tasks].sort((a, b) => a.orderIndex - b.orderIndex);

    return (
        <div className="flex flex-col w-80 shrink-0 bg-zinc-50 dark:bg-zinc-900/50 rounded-xl border border-zinc-200/50 dark:border-zinc-800/50 h-full overflow-hidden">
            <div className="p-4 border-b border-zinc-200 dark:border-zinc-800 flex items-center justify-between bg-zinc-100/50 dark:bg-zinc-900/80">
                <div className="flex items-center gap-2">
                    <div className="w-3 h-3 rounded-full" style={{ backgroundColor: state.color || '#6366f1' }} />
                    <h3 className="font-semibold text-sm text-zinc-700 dark:text-zinc-200">{state.name}</h3>
                    <span className="text-xs font-medium text-zinc-500 bg-zinc-200 dark:bg-zinc-800 px-2 py-0.5 rounded-full">
                        {tasks.length}
                    </span>
                </div>
            </div>

            <Droppable droppableId={state.id}>
                {(provided, snapshot) => (
                    <div
                        ref={provided.innerRef}
                        {...provided.droppableProps}
                        className={`flex-1 p-3 overflow-y-auto min-h-[150px] space-y-3 transition-colors ${
                            snapshot.isDraggingOver ? 'bg-zinc-100 dark:bg-zinc-800/50' : ''
                        }`}
                    >
                        {sortedTasks.map((task, index) => (
                            <TaskCard key={task.id} task={task} index={index} />
                        ))}
                        {provided.placeholder}
                    </div>
                )}
            </Droppable>
        </div>
    );
};