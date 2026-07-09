import { useQuery } from '@tanstack/react-query';
import { activityApi } from '../api/activity.api';
import { ActivityQueryParams } from '../types/activity.types';

export const ACTIVITY_KEYS = {
    all: ['activity'] as const,
    list: (params: ActivityQueryParams) => ['activity', 'list', params] as const,
};

export const useActivityFeedQuery = (params: ActivityQueryParams, enabled: boolean = true) => {
    return useQuery({
        queryKey: ACTIVITY_KEYS.list(params),
        queryFn: () => activityApi.getLogs(params),
        enabled: enabled && Object.keys(params).length > 0, // Ensure at least one scope is provided
        refetchInterval: 30000, // Auto-refresh every 30 seconds for near-realtime feel
    });
};