import { apiClient } from '@/api/client';
import { ActivityLogDto, ActivityQueryParams } from '../types/activity.types';

export const activityApi = {
    getLogs: async (params: ActivityQueryParams): Promise<ActivityLogDto[]> => {
        // Convert params to query string
        const queryParams = new URLSearchParams();
        if (params.workspaceId) queryParams.append('workspaceId', params.workspaceId);
        if (params.projectId) queryParams.append('projectId', params.projectId);
        if (params.taskId) queryParams.append('taskId', params.taskId);
        if (params.userId) queryParams.append('userId', params.userId);
        if (params.limit) queryParams.append('limit', params.limit.toString());

        const { data } = await apiClient.get<ActivityLogDto[]>(`/activity?${queryParams.toString()}`);
        return data;
    }
};