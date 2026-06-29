import { apiClient } from '@/api/client';
import {
    SprintVelocityDto,
    BurndownDataPointDto,
    CycleTimeDto,
    CumulativeFlowDataPointDto
} from '../types/analytics.types';

export const analyticsApi = {
    getSprintVelocity: async (projectId: string): Promise<SprintVelocityDto[]> => {
        const { data } = await apiClient.get<SprintVelocityDto[]>(`/analytics/projects/${projectId}/velocity`);
        return data;
    },

    getBurndown: async (sprintId: string): Promise<BurndownDataPointDto[]> => {
        const { data } = await apiClient.get<BurndownDataPointDto[]>(`/analytics/sprints/${sprintId}/burndown`);
        return data;
    },

    getCycleTime: async (projectId: string): Promise<CycleTimeDto[]> => {
        const { data } = await apiClient.get<CycleTimeDto[]>(`/analytics/projects/${projectId}/cycle-time`);
        return data;
    },

    getCumulativeFlow: async (projectId: string): Promise<CumulativeFlowDataPointDto[]> => {
        const { data } = await apiClient.get<CumulativeFlowDataPointDto[]>(`/analytics/projects/${projectId}/cumulative-flow`);
        return data;
    }
};