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

export const SprintBoard = ({ workspaceId, projectId }: { workspaceId: string, projectId: string }) => {
    const { data: sprints, isLoading: sprintsLoading } = useSprintsQuery(projectId);
    const { data: tasks, isLoading: tasksLoading } = useTasksQuery(projectId);
    const { mutate: assignTask } = useAssignTaskMutation(projectId);
    const { startMutation, completeMutation } = useSprintMutations(projectId);

    const [isMounted, setIsMounted] = useState(false);
    useEffect(() => { setIsMounted(true); }, []);

    if (!isMounted || sprintsLoading || tasksLoading) return <Skeleton className="w-full h-[500px]" />;

    const onDragEnd = (result: DropResult) => {
        const { destination, source, draggableId } = result;
        if (!destination) return;
        if (destination.droppableId === source.droppableId) return; // Ignore reordering within same sprint for now

        const targetSprintId = destination.droppableId === 'backlog' ? null : destination.droppableId;
        assignTask({ taskId: draggableId, sprintId: targetSprintId });
    };

    const backlogTasks = tasks?.filter(t => !t.sprintId) || [];
    const activeAndPlannedSprints = sprints?.filter(s => s.status === SprintStatus.Active || s.status === SprintStatus.Planned) || [];

    const renderTaskContainer = (droppableId: string, taskList: TaskDto[], isBacklog: boolean = false) => (
        <div className="flex flex-col">
            <Droppable droppableId={droppableId}>
                {(provided, snapshot) => (
                    <div 
                        ref={provided.innerRef} 
                        {...provided.droppableProps}
                        className={`min-h-[100px] p-2 rounded-lg transition-colors ${snapshot.isDraggingOver ? 'bg-zinc-100 dark:bg-zinc-800' : ''}`}
                    >
                        {taskList.map((task, index) => (
                            <div key={task.id} className="mb-2">
                                <TaskCard task={task} index={index} workspaceId={workspaceId} projectId={projectId} />
                            </div>
                        ))}
                        {provided.placeholder}
                        {taskList.length === 0 && <div className="text-center p-4 border-2 border-dashed rounded-lg text-sm text-zinc-500 mb-2">Drop tasks here</div>}
                    </div>
                )}
            </Droppable>
            {/* YENİ: Sprint ve Backlog kutularının altına Task Ekle butonu koyuyoruz */}
            <div className="px-2 mt-1">
                <CreateTaskDialog 
                    workspaceId={workspaceId} 
                    projectId={projectId} 
                    // Eğer Backlog değilse, sprintId'yi göndermemiz lazım ama CreateTaskDialog bunu desteklemiyor şu an. 
                    // O yüzden en azından genel bir Task oluşturma butonu ekliyoruz.
                />
            </div>
        </div>
    );

    return (
        <DragDropContext onDragEnd={onDragEnd}>
            <div className="flex flex-col gap-8 max-w-4xl mx-auto pb-12">
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
                            
                            <div className="bg-zinc-50 dark:bg-zinc-900/50 rounded-lg p-2 border border-zinc-200/50 dark:border-zinc-800/50">
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
                    
                    <div className="bg-zinc-50 dark:bg-zinc-900/50 rounded-lg p-2 border border-zinc-200/50 dark:border-zinc-800/50">
                        {renderTaskContainer('backlog', backlogTasks)}
                    </div>
                </div>

            </div>
        </DragDropContext>
    );
};