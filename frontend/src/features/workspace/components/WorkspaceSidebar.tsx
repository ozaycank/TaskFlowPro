'use client';

import { WorkspaceSwitcher } from './WorkspaceSwitcher';
import { Settings, Users, FolderKanban, Activity, ListTodo, BarChart2, Book } from 'lucide-react';
import Link from 'next/link';
import { useWorkspaceStore } from '../stores/useWorkspaceStore';
import { usePathname, useParams } from 'next/navigation';
import { NotificationBell } from '@/features/notifications/components/NotificationBell';
import { GlobalSearchModal } from '@/features/search/components/GlobalSearchModal';

export const WorkspaceSidebar = () => {
  const { activeWorkspaceId } = useWorkspaceStore();
  const pathname = usePathname();
  
  const params = useParams();
  const projectId = params.projectId as string | undefined;

  const getNavLinks = () => {
    if (!activeWorkspaceId) return [];
    const base = `/workspaces/${activeWorkspaceId}`;
    
    const links = [
      { name: 'Dashboard', href: base, icon: Activity },
      { name: 'Projects', href: `${base}/projects`, icon: FolderKanban },
      { name: 'Members', href: `${base}/members`, icon: Users },
      { name: 'Documents', href: `${base}/documents`, icon: Book },
    ];

    if (projectId) {
      links.push({ name: 'Sprints & Backlog', href: `${base}/projects/${projectId}/sprints`, icon: ListTodo });
      links.push({ name: 'Kanban Board', href: `${base}/projects/${projectId}`, icon: FolderKanban });
      links.push({ name: 'Analytics', href: `${base}/projects/${projectId}/analytics`, icon: BarChart2 });
    }

    links.push({ name: 'Settings', href: `${base}/settings`, icon: Settings });
    return links;
  };

  const navLinks = getNavLinks();

  return (
    <div className="w-64 border-r border-zinc-200 dark:border-zinc-800 bg-zinc-50/50 dark:bg-zinc-950/50 flex flex-col h-full">
      <div className="p-4 border-b border-zinc-200 dark:border-zinc-800 flex justify-between items-center">
        <div className="flex-1 min-w-0">
          <WorkspaceSwitcher />
        </div>
        <div className="ml-2 flex-shrink-0">
          <NotificationBell />
        </div>
      </div>
      
      {activeWorkspaceId && (
        <div className="px-4 pt-4 pb-1">
          <GlobalSearchModal />
        </div>
      )}

      <nav className="flex-1 px-4 py-2 space-y-1 overflow-y-auto">
        {navLinks.map((link) => {
          const Icon = link.icon;
          const isActive = pathname === link.href || (link.href !== `/workspaces/${activeWorkspaceId}` && pathname.startsWith(link.href));
          
          return (
            <Link
              key={link.name}
              href={link.href}
              className={`flex items-center gap-3 px-3 py-2 rounded-md text-sm transition-colors ${
                isActive 
                  ? 'bg-zinc-200 dark:bg-zinc-800 font-medium text-zinc-900 dark:text-white' 
                  : 'text-zinc-600 dark:text-zinc-400 hover:bg-zinc-100 dark:hover:bg-zinc-800/50 hover:text-zinc-900 dark:hover:text-white'
              }`}
            >
              <Icon size={18} />
              {link.name}
            </Link>
          );
        })}
      </nav>
    </div>
  );
};