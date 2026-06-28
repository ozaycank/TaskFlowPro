import { useQueryClient } from '@tanstack/react-query';
import { useSignalREvent } from '@/signalr/hooks/useSignalREvent';
import { NOTIFICATION_KEYS } from './useNotificationHooks';
import { NotificationDto } from '../types/notification.types';

export const useNotificationRealtimeUpdates = () => {
    const queryClient = useQueryClient();

    useSignalREvent<NotificationDto>('ReceiveNotification', (newNotification) => {
        // Optimistically add to both caches

        queryClient.setQueryData<NotificationDto[]>(NOTIFICATION_KEYS.all, (oldData) => {
            if (!oldData) return [newNotification];
            return [newNotification, ...oldData]; // Prepend newest
        });

        queryClient.setQueryData<NotificationDto[]>(NOTIFICATION_KEYS.unread, (oldData) => {
            if (!oldData) return [newNotification];
            return [newNotification, ...oldData];
        });

        // In a real app, you could also trigger a Toast notification here:
        // toast({ title: newNotification.title, description: newNotification.message });
    });
};