import { apiClient } from '@/api/client';
import { NotificationDto } from '../types/notification.types';

export const notificationApi = {
    getMyNotifications: async (unreadOnly: boolean = false): Promise<NotificationDto[]> => {
        const response = await apiClient.get<NotificationDto[]>(`/notifications?unreadOnly=${unreadOnly}`);
        return response.data;
    },

    markAsRead: async (notificationId: string): Promise<void> => {
        await apiClient.put(`/notifications/${notificationId}/read`);
    },

    markAllAsRead: async (): Promise<void> => {
        await apiClient.put('/notifications/read-all');
    }
};