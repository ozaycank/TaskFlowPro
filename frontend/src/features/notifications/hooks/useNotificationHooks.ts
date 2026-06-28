import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { notificationApi } from '../api/notification.api';

export const NOTIFICATION_KEYS = {
    all: ['notifications'] as const,
    unread: ['notifications', 'unread'] as const,
};

export const useNotificationsQuery = (unreadOnly: boolean = false) => {
    return useQuery({
        queryKey: unreadOnly ? NOTIFICATION_KEYS.unread : NOTIFICATION_KEYS.all,
        queryFn: () => notificationApi.getMyNotifications(unreadOnly),
        staleTime: 1000 * 60 * 5, // 5 minutes (SignalR handles real-time invalidation)
    });
};

export const useMarkNotificationAsReadMutation = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (id: string) => notificationApi.markAsRead(id),
        onMutate: async (notificationId) => {
            // Optimistic Update
            await queryClient.cancelQueries({ queryKey: NOTIFICATION_KEYS.all });
            await queryClient.cancelQueries({ queryKey: NOTIFICATION_KEYS.unread });

            const previousAll = queryClient.getQueryData<any[]>(NOTIFICATION_KEYS.all);

            if (previousAll) {
                queryClient.setQueryData(NOTIFICATION_KEYS.all, (old: any[]) =>
                    old.map(n => n.id === notificationId ? { ...n, isRead: true } : n)
                );
            }

            // Remove from unread cache if it exists
            queryClient.setQueryData(NOTIFICATION_KEYS.unread, (old: any[]) =>
                old?.filter(n => n.id !== notificationId) ?? []
            );

            return { previousAll };
        },
        onError: (err, newTodo, context) => {
            if (context?.previousAll) {
                queryClient.setQueryData(NOTIFICATION_KEYS.all, context.previousAll);
            }
        },
        onSettled: () => {
            queryClient.invalidateQueries({ queryKey: NOTIFICATION_KEYS.all });
            queryClient.invalidateQueries({ queryKey: NOTIFICATION_KEYS.unread });
        }
    });
};

export const useMarkAllNotificationsAsReadMutation = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: () => notificationApi.markAllAsRead(),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: NOTIFICATION_KEYS.all });
            queryClient.invalidateQueries({ queryKey: NOTIFICATION_KEYS.unread });
        },
    });
};