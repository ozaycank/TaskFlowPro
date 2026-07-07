import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { timeTrackingApi } from '../api/time-tracking.api';
import { CreateWorklogCommand, UpdateWorklogCommand } from '../types/time-tracking.types';

export const TIME_TRACKING_KEYS = {
    worklogs: (taskId: string) => ['time-tracking', 'worklogs', taskId] as const,
};

export const useWorklogsQuery = (taskId: string | undefined) => {
    return useQuery({
        queryKey: TIME_TRACKING_KEYS.worklogs(taskId!),
        queryFn: () => timeTrackingApi.getWorklogsByTask(taskId!),
        enabled: !!taskId,
    });
};

export const useCreateWorklogMutation = (taskId: string) => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (command: CreateWorklogCommand) => timeTrackingApi.createWorklog(command),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: TIME_TRACKING_KEYS.worklogs(taskId) });
        },
    });
};

export const useDeleteWorklogMutation = (taskId: string) => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (worklogId: string) => timeTrackingApi.deleteWorklog(worklogId),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: TIME_TRACKING_KEYS.worklogs(taskId) });
        },
    });
};