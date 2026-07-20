'use client';

import { useEffect, useState } from 'react';
import { DragDropContext, Droppable, DropResult } from '@hello-pangea/dnd';
import { useSprintsQuery } from '../hooks/useSprintsQuery';
import { useTasksQuery } from '@/features/workflows/hooks/useTasksQuery';
import { useAssignTaskMutation } from '@/features/sprints/hooks/useAssignTaskMutation';
import { TaskCard } from '@/features/tasks/components/TaskCard';
import { CreateSprintDialog } from './CreateSprintDialog';
import { SprintStatus } from '../types/sprint.types';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { Skeleton } from '@/components/ui/skeleton';
import { format } from 'date-fns';
import { useSprintMutations } from '../hooks/useSprintMutations';
import { TaskDto } from '@/features/workflows/types/task.types';
import { CreateTaskDialog } from '@/features/tasks/components/CreateTaskDialog';
import { useQueryClient } from '@tanstack/react-query';

export const SprintBoard = ({ workspaceId, projectId }: { workspaceId: string, projectId: string }) => {
    const { data: sprints, isLoading: sprintsLoading } = useSprintsQuery(projectId);
    const { data: tasks, isLoading: tasksLoading } = useTasksQuery(projectId);
    const { mutate: assignTask } = useAssignTaskMutation(projectId);
    const { startMutation, completeMutation } = useSprintMutations(projectId);
    const queryClient = useQueryClient();

    const [isMounted, setIsMounted] = useState(false);
    useEffect(() => { setIsMounted(true); }, []);

    if (!isMounted || sprintsLoading || tasksLoading) return <Skeleton className="w-full h-[500px]" />;

    const onDragEnd = (result: DropResult) => {
        const { destination, source, draggableId } = result;

        // 1. Dışarı bırakılırsa hiçbir şey yapma
        if (!destination) return;

        // 2. Aynı listeye bırakılırsa (şimdilik) hiçbir şey yapma
        if (destination.droppableId === source.droppableId) return;

        // 3. Hedef Sprint ID'sini belirle (Backlog ise null olacak)
        const targetSprintId = destination.droppableId === 'backlog' ? null : destination.droppableId;

        // --- OPTIMISTIC UPDATE ---
        // UI'ın anında tepki vermesi ve kartın geri zıplamasını engellemek için cache'i manuel güncelliyoruz.
        queryClient.setQueryData(['tasks', projectId], (oldData: TaskDto[] | undefined) => {
            if (!oldData) return [];
            return oldData.map(task => 
                task.id === draggableId 
                    ? { ...task, sprintId: targetSprintId } 
                    : task
            );
        });

        // 4. Arka planda Backend mutasyonunu tetikle
        assignTask({ taskId: draggableId, sprintId: targetSprintId });
    };

    const backlogTasks = tasks?.filter(t => !t.sprintId) || [];
    const activeAndPlannedSprints = sprints?.filter(s => s.status === SprintStatus.Active || s.status === SprintStatus.Planned) || [];

    const renderTaskContainer = (droppableId: string, taskList: TaskDto[], isBacklog: boolean = false) => (
        <div className="flex flex-col h-full">
            <Droppable droppableId={droppableId} ignoreContainerClipping={true}>
                {(provided, snapshot) => (
                    <div 
                        ref={provided.innerRef} 
                        {...provided.droppableProps}
                        className={`min-h-[120px] p-2 rounded-lg transition-colors flex-1 ${snapshot.isDraggingOver ? 'bg-zinc-100 dark:bg-zinc-800' : ''}`}
                    >
                        {taskList.map((task, index) => (
                            <div key={task.id} className="mb-2">
                                {/* Draggable wrapper TaskCard içinde tanımlanmış olmalı. Eğer sorun çıkarsa buraya taşıyabiliriz. */}
                                <TaskCard task={task} index={index} workspaceId={workspaceId} projectId={projectId} />
                            </div>
                        ))}
                        {provided.placeholder}
                        {taskList.length === 0 && !snapshot.isDraggingOver && (
                            <div className="text-center p-4 border-2 border-dashed rounded-lg text-sm text-zinc-500 mb-2">
                                Drop tasks here
                            </div>
                        )}
                    </div>
                )}
            </Droppable>
            <div className="px-2 mt-1">
                <CreateTaskDialog 
                    workspaceId={workspaceId} 
                    projectId={projectId} 
                />
            </div>
        </div>
    );

    return (
        <DragDropContext onDragEnd={onDragEnd}>
            <div className="flex flex-col gap-8 max-w-4xl mx-auto pb-12 overflow-x-hidden">
                <div className="flex justify-between items-center">
                    <h2 className="text-2xl font-bold">Agile Planning</h2>
                    <CreateSprintDialog projectId={projectId} />
                </div>

                {/* SPRINTS */}
                {activeAndPlannedSprints.map(sprint => {
                    const sprintTasks = tasks?.filter(t => t.sprintId === sprint.id) || [];
                    const isActive = sprint.status === SprintStatus.Active;
                    
                    return (
                        <div key={sprint.id} className={`border rounded-xl p-4 shadow-sm ${isActive ? 'border-indigo-500 dark:border-indigo-500/50 bg-indigo-50/10' : 'border-zinc-200 dark:border-zinc-800 bg-white dark:bg-zinc-950'}`}>
                            <div className="flex justify-between items-start mb-4">
                                <div>
                                    <div className="flex items-center gap-2">
                                        <h3 className="font-bold text-lg">{sprint.name}</h3>
                                        <Badge variant={isActive ? "default" : "secondary"}>
                                            {SprintStatus[sprint.status]}
                                        </Badge>
                                        <span className="text-xs text-zinc-500 bg-zinc-100 dark:bg-zinc-800 px-2 py-0.5 rounded-full">
                                            {sprintTasks.length} issues
                                        </span>
                                    </div>
                                    <p className="text-xs text-zinc-500 mt-1">
                                        {sprint.startDate && sprint.endDate 
                                            ? `${format(new Date(sprint.startDate), 'MMM d')} - ${format(new Date(sprint.endDate), 'MMM d, yyyy')}` 
                                            : 'No dates set'}
                                    </p>
                                    {sprint.goal && <p className="text-sm mt-2 font-medium">Goal: {sprint.goal}</p>}
                                </div>
                                
                                {isActive ? (
                                    <Button size="sm" variant="outline" onClick={() => completeMutation.mutate(sprint.id)} disabled={completeMutation.isPending}>
                                        Complete Sprint
                                    </Button>
                                ) : (
                                    <Button size="sm" onClick={() => startMutation.mutate(sprint.id)} disabled={startMutation.isPending || sprintTasks.length === 0}>
                                        Start Sprint
                                    </Button>
                                )}
                            </div>
                            
                            <div className="bg-zinc-50 dark:bg-zinc-900/50 rounded-lg p-2 border border-zinc-200/50 dark:border-zinc-800/50 min-h-[150px]">
                                {renderTaskContainer(sprint.id, sprintTasks)}
                            </div>
                        </div>
                    );
                })}

                {/* BACKLOG */}
                <div className="border border-zinc-200 dark:border-zinc-800 bg-white dark:bg-zinc-950 rounded-xl p-4 shadow-sm">
                    <div className="flex justify-between items-center mb-4">
                        <div className="flex items-center gap-2">
                            <h3 className="font-bold text-lg">Backlog</h3>
                            <span className="text-xs text-zinc-500 bg-zinc-100 dark:bg-zinc-800 px-2 py-0.5 rounded-full">
                                {backlogTasks.length} issues
                            </span>
                        </div>
                    </div>
                    
                    <div className="bg-zinc-50 dark:bg-zinc-900/50 rounded-lg p-2 border border-zinc-200/50 dark:border-zinc-800/50 min-h-[150px]">
                        {renderTaskContainer('backlog', backlogTasks, true)}
                    </div>
                </div>

            </div>
        </DragDropContext>
    );
};