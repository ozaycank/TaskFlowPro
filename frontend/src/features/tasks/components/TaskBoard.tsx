'use client';

import { useEffect, useState } from 'react';
import { DragDropContext, DropResult } from '@hello-pangea/dnd';
import { useParams } from 'next/navigation';
import { useWorkflowStatesQuery } from '@/features/workflows/hooks/useWorkflowStatesQuery';
import { useTasksQuery } from '../../workflows/hooks/useTasksQuery';
import { useTransitionTaskMutation } from '../../workflows/hooks/useTransitionTaskMutation';
import { TaskColumn } from '../components/TaskColumn';
import { TaskDto } from '../../workflows/types/task.types';

export const TaskBoard = () => {
    const params = useParams();
    const workspaceId = params.workspaceId as string;
    const projectId = params.projectId as string; // Adjust based on your actual route param name

    const { data: states, isLoading: statesLoading } = useWorkflowStatesQuery(workspaceId);
    const { data: tasks, isLoading: tasksLoading } = useTasksQuery(projectId);
    const { mutate: transitionTask } = useTransitionTaskMutation(projectId);

    // Strict Mode Hydration Fix for DnD
    const [isMounted, setIsMounted] = useState(false);
    useEffect(() => {
        setIsMounted(true);
    }, []);

    if (!isMounted || statesLoading || tasksLoading) {
        return <div className="w-full h-full flex items-center justify-center animate-pulse text-zinc-500">Loading Board...</div>;
    }

    if (!states || states.length === 0) {
        return <div className="p-8 text-center text-zinc-500">No workflow states defined for this workspace.</div>;
    }

    const onDragEnd = (result: DropResult) => {
        const { destination, source, draggableId } = result;

        // Dropped outside a column or exactly where it started
        if (!destination) return;
        if (destination.droppableId === source.droppableId && destination.index === source.index) return;

        const targetStateId = destination.droppableId;
        const targetTasks = tasks?.filter(t => t.stateId === targetStateId).sort((a, b) => a.orderIndex - b.orderIndex) || [];

        let newOrderIndex = 0;

        // Enterprise LexoRank / Fractional Indexing Logic approximation
        if (targetTasks.length === 0) {
            newOrderIndex = 1000; // First item in empty column
        } else if (destination.index === 0) {
            newOrderIndex = targetTasks[0].orderIndex / 2; // Placed at the very top
        } else if (destination.index >= targetTasks.length) {
            newOrderIndex = targetTasks[targetTasks.length - 1].orderIndex + 1000; // Placed at the bottom
        } else {
            // Placed between two items
            const prev = targetTasks[destination.index - 1].orderIndex;
            const next = targetTasks[destination.index].orderIndex;
            newOrderIndex = (prev + next) / 2;
        }

        // Fire Optimistic Update to Backend
        transitionTask({
            taskId: draggableId,
            newStateId: targetStateId,
            newOrderIndex: newOrderIndex
        });
    };

    // Sort states so columns appear in correct order
    const sortedStates = [...states].sort((a, b) => a.orderIndex - b.orderIndex);

    return (
        <DragDropContext onDragEnd={onDragEnd}>
            <div className="flex h-full gap-6 overflow-x-auto pb-4 items-start">
                {sortedStates.map((state) => (
                    <TaskColumn 
                        key={state.id} 
                        state={state} 
                        tasks={tasks?.filter((t) => t.stateId === state.id) || []} 
                    />
                ))}
            </div>
        </DragDropContext>
    );
};