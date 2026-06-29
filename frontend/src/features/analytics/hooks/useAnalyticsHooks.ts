import { useQuery } from '@tanstack/react-query';
import { analyticsApi } from '../api/analytics.api';

export const ANALYTICS_KEYS = {
    velocity: (projectId: string) => ['analytics', 'velocity', projectId] as const,
    burndown: (sprintId: string) => ['analytics', 'burndown', sprintId] as const,
    cycleTime: (projectId: string) => ['analytics', 'cycle-time', projectId] as const,
    cfd: (projectId: string) => ['analytics', 'cfd', projectId] as const,
};

export const useVelocityQuery = (projectId: string | undefined) => {
    return useQuery({
        queryKey: ANALYTICS_KEYS.velocity(projectId!),
        queryFn: () => analyticsApi.getSprintVelocity(projectId!),
        enabled: !!projectId,
    });
};

export const useBurndownQuery = (sprintId: string | undefined) => {
    return useQuery({
        queryKey: ANALYTICS_KEYS.burndown(sprintId!),
        queryFn: () => analyticsApi.getBurndown(sprintId!),
        enabled: !!sprintId,
    });
};

export const useCycleTimeQuery = (projectId: string | undefined) => {
    return useQuery({
        queryKey: ANALYTICS_KEYS.cycleTime(projectId!),
        queryFn: () => analyticsApi.getCycleTime(projectId!),
        enabled: !!projectId,
    });
};

export const useCFDQuery = (projectId: string | undefined) => {
    return useQuery({
        queryKey: ANALYTICS_KEYS.cfd(projectId!),
        queryFn: () => analyticsApi.getCumulativeFlow(projectId!),
        enabled: !!projectId,
    });
};