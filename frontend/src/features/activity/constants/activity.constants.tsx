import { 
    CheckCircle2, 
    Plus, 
    Trash2, 
    Edit3, 
    UserPlus, 
    MessageSquare, 
    Paperclip, 
    FolderKanban, 
    Activity 
} from 'lucide-react';

export const getActivityIcon = (entityType: string, action: string) => {
    const key = `${entityType}.${action}`.toLowerCase();
    
    // Status changes
    if (key.includes('status') || key.includes('completed')) return <CheckCircle2 size={16} className="text-emerald-500" />;
    
    // Creations
    if (key.includes('created') || key.includes('added')) return <Plus size={16} className="text-indigo-500" />;
    
    // Updates
    if (key.includes('updated') || key.includes('renamed')) return <Edit3 size={16} className="text-amber-500" />;
    
    // Deletions
    if (key.includes('deleted') || key.includes('removed')) return <Trash2 size={16} className="text-red-500" />;
    
    // Specific entities
    if (entityType.toLowerCase() === 'user' || entityType.toLowerCase() === 'member') return <UserPlus size={16} className="text-blue-500" />;
    if (entityType.toLowerCase() === 'comment') return <MessageSquare size={16} className="text-purple-500" />;
    if (entityType.toLowerCase() === 'attachment') return <Paperclip size={16} className="text-slate-500" />;
    if (entityType.toLowerCase() === 'project') return <FolderKanban size={16} className="text-orange-500" />;

    // Fallback
    return <Activity size={16} className="text-zinc-400" />;
};