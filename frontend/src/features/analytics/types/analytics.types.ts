export interface SprintVelocityDto {
    sprintId: string;
    sprintName: string;
    completedTasks: number;
    totalTasks: number;
}

export interface BurndownDataPointDto {
    date: string;
    remainingTasks: number;
    idealTasks: number;
}

export interface CycleTimeDto {
    taskId: string;
    taskTitle: string;
    leadTimeDays: number;
    cycleTimeDays: number;
}

export interface CumulativeFlowDataPointDto {
    date: string;
    stateName: string;
    taskCount: number;
}