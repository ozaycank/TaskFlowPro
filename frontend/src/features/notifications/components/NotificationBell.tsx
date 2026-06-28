'use client';

import { useState } from 'react';
import { Bell } from 'lucide-react';
import { DropdownMenu, DropdownMenuContent, DropdownMenuTrigger } from '@/components/ui/dropdown-menu';
import { Button } from '@/components/ui/button';
import { 
    useNotificationsQuery, 
    useMarkNotificationAsReadMutation, 
    useMarkAllNotificationsAsReadMutation 
} from '../hooks/useNotificationHooks';
import { useNotificationRealtimeUpdates } from '../hooks/useNotificationRealtimeUpdates';
import { NotificationCard } from './NotificationCard';
import { Skeleton } from '@/components/ui/skeleton';

export const NotificationBell = () => {
    const [isOpen, setIsOpen] = useState(false);
    
    // Always listen to SignalR events globally as long as this component is mounted
    useNotificationRealtimeUpdates();

    // Fetch unread count for the badge (lightweight)
    const { data: unreadNotifications, isLoading: loadingUnread } = useNotificationsQuery(true);
    
    // Fetch all notifications for the dropdown list (only if opened)
    const { data: allNotifications, isLoading: loadingAll } = useNotificationsQuery(false);

    const { mutate: markAsRead } = useMarkNotificationAsReadMutation();
    const { mutate: markAllAsRead, isPending: markingAll } = useMarkAllNotificationsAsReadMutation();

    const unreadCount = unreadNotifications?.length || 0;

    return (
        <DropdownMenu open={isOpen} onOpenChange={setIsOpen}>
            <DropdownMenuTrigger asChild>
                <Button variant="ghost" size="icon" className="relative">
                    <Bell className="h-5 w-5 text-zinc-600 dark:text-zinc-400" />
                    {!loadingUnread && unreadCount > 0 && (
                        <span className="absolute top-1 right-1.5 flex h-4 w-4 items-center justify-center rounded-full bg-red-500 text-[10px] font-bold text-white shadow-sm ring-2 ring-white dark:ring-zinc-950">
                            {unreadCount > 9 ? '9+' : unreadCount}
                        </span>
                    )}
                </Button>
            </DropdownMenuTrigger>
            
            <DropdownMenuContent align="end" className="w-80 p-0 shadow-lg border-zinc-200 dark:border-zinc-800">
                <div className="flex items-center justify-between px-4 py-3 border-b border-zinc-100 dark:border-zinc-800">
                    <h3 className="font-semibold text-sm">Notifications</h3>
                    {unreadCount > 0 && (
                        <button 
                            onClick={() => markAllAsRead()}
                            disabled={markingAll}
                            className="text-xs text-indigo-600 dark:text-indigo-400 font-medium hover:underline disabled:opacity-50"
                        >
                            Mark all as read
                        </button>
                    )}
                </div>

                <div className="max-h-[400px] overflow-y-auto">
                    {loadingAll ? (
                        <div className="p-4 space-y-3">
                            {[1, 2, 3].map(i => <Skeleton key={i} className="h-12 w-full" />)}
                        </div>
                    ) : !allNotifications || allNotifications.length === 0 ? (
                        <div className="p-8 text-center text-zinc-500">
                            <Bell className="mx-auto h-8 w-8 text-zinc-300 dark:text-zinc-700 mb-2" />
                            <p className="text-sm">You're all caught up!</p>
                        </div>
                    ) : (
                        <div className="flex flex-col">
                            {allNotifications.map(notification => (
                                <NotificationCard 
                                    key={notification.id} 
                                    notification={notification}
                                    onRead={(id) => markAsRead(id)}
                                    onClose={() => setIsOpen(false)}
                                />
                            ))}
                        </div>
                    )}
                </div>
            </DropdownMenuContent>
        </DropdownMenu>
    );
};