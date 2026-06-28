import { NotificationDto, NotificationType } from '../types/notification.types';
import { formatDistanceToNow } from 'date-fns';
import { Check, Bell, UserPlus, Briefcase, CheckCircle2, MessageSquare } from 'lucide-react';
import { useRouter } from 'next/navigation';

interface Props {
    notification: NotificationDto;
    onRead: (id: string) => void;
    onClose: () => void;
}

const getIcon = (type: NotificationType) => {
    switch (type) {
        case NotificationType.WorkspaceInvitation: return <UserPlus className="text-blue-500" size={16} />;
        case NotificationType.ProjectCreated: return <Briefcase className="text-purple-500" size={16} />;
        case NotificationType.TaskStatusChanged: return <CheckCircle2 className="text-green-500" size={16} />;
        case NotificationType.Mentioned: return <MessageSquare className="text-amber-500" size={16} />;
        default: return <Bell className="text-zinc-500" size={16} />;
    }
};

export const NotificationCard = ({ notification, onRead, onClose }: Props) => {
    const router = useRouter();

    const handleClick = () => {
        if (!notification.isRead) {
            onRead(notification.id);
        }
        if (notification.actionUrl) {
            router.push(notification.actionUrl);
            onClose(); // Close dropdown after navigation
        }
    };

    return (
        <div 
            onClick={handleClick}
            className={`p-3 flex gap-3 cursor-pointer hover:bg-zinc-50 dark:hover:bg-zinc-800 transition-colors border-b last:border-0 border-zinc-100 dark:border-zinc-800/50 ${!notification.isRead ? 'bg-indigo-50/30 dark:bg-indigo-900/10' : ''}`}
        >
            <div className="mt-1 flex-shrink-0">
                <div className="w-8 h-8 rounded-full bg-white dark:bg-zinc-900 border border-zinc-200 dark:border-zinc-700 flex items-center justify-center">
                    {getIcon(notification.type)}
                </div>
            </div>
            <div className="flex-1 min-w-0">
                <p className={`text-sm ${!notification.isRead ? 'font-semibold text-zinc-900 dark:text-zinc-100' : 'text-zinc-700 dark:text-zinc-300'}`}>
                    {notification.title}
                </p>
                <p className="text-xs text-zinc-500 line-clamp-2 mt-0.5">{notification.message}</p>
                <span className="text-[10px] text-zinc-400 mt-1 block">
                    {formatDistanceToNow(new Date(notification.createdAt), { addSuffix: true })}
                </span>
            </div>
            {!notification.isRead && (
                <div className="flex-shrink-0 flex items-center">
                    <button 
                        onClick={(e) => { e.stopPropagation(); onRead(notification.id); }}
                        className="p-1.5 rounded-full text-indigo-500 hover:bg-indigo-100 dark:hover:bg-indigo-900/30 transition-colors"
                        title="Mark as read"
                    >
                        <Check size={14} />
                    </button>
                </div>
            )}
        </div>
    );
};