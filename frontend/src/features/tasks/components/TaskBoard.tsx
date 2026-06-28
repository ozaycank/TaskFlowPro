'use client';

import { useEffect, useState } from 'react';
import { DragDropContext, DropResult } from '@hello-pangea/dnd';
import { useParams } from 'next/navigation';
import { useWorkflowStatesQuery } from '@/features/workflows/hooks/useWorkflowStatesQuery';
import { useTasksQuery } from '../../workflows/hooks/useTasksQuery';
import { useTransitionTaskMutation } from '../../workflows/hooks/useTransitionTaskMutation';
import { useTaskRealtimeUpdates } from '../hooks/useTaskRealtimeUpdates';
import { TaskColumn } from './TaskColumn';
import { TaskDto } from '../../workflows/types/task.types';

export const TaskBoard = () => {
    const params = useParams();
    const workspaceId = params.workspaceId as string;
    const projectId = params.projectId as string;

    const { data: states, isLoading: statesLoading } = useWorkflowStatesQuery(workspaceId);
    const { data: tasks, isLoading: tasksLoading } = useTasksQuery(projectId);
    const { mutate: transitionTask } = useTransitionTaskMutation(projectId);

    useTaskRealtimeUpdates();

    const [isMounted, setIsMounted] = useState(false);
    useEffect(() => {
        setIsMounted(true);
    }, []);

    if (!isMounted || statesLoading || tasksLoading) {
        return <div className="w-full h-full flex items-center justify-center animate-pulse text-zinc-500">Loading Board...</div>;
    }

    if (!states || states.length === 0) {
        return <div className="p-8 text-center text-zinc-500">No workflow states defined for this workspace. Please re-create the workspace to trigger default workflows.</div>;
    }

    const onDragEnd = (result: DropResult) => {
        const { destination, source, draggableId } = result;

        if (!destination) return;
        if (destination.droppableId === source.droppableId && destination.index === source.index) return;

        const targetStateId = destination.droppableId;
        const targetTasks = tasks?.filter(t => t.stateId === targetStateId).sort((a, b) => a.orderIndex - b.orderIndex) || [];

        let newOrderIndex = 0;

        if (targetTasks.length === 0) {
            newOrderIndex = 1000;
        } else if (destination.index === 0) {
            newOrderIndex = targetTasks[0].orderIndex / 2;
        } else if (destination.index >= targetTasks.length) {
            newOrderIndex = targetTasks[targetTasks.length - 1].orderIndex + 1000;
        } else {
            const prev = targetTasks[destination.index - 1].orderIndex;
            const next = targetTasks[destination.index].orderIndex;
            newOrderIndex = (prev + next) / 2;
        }

        transitionTask({
            taskId: draggableId,
            newStateId: targetStateId,
            newOrderIndex: newOrderIndex
        });
    };

    const sortedStates = [...states].sort((a, b) => a.orderIndex - b.orderIndex);

    return (
        <DragDropContext onDragEnd={onDragEnd}>
            <div className="flex h-full gap-6 overflow-x-auto pb-4 items-start">
                {sortedStates.map((state) => (
                    <TaskColumn 
                        key={state.id} 
                        state={state} 
                        tasks={tasks?.filter((t) => t.stateId === state.id) || []}
                        workspaceId={workspaceId}
                        projectId={projectId}
                    />
                ))}
            </div>
        </DragDropContext>
    );
};