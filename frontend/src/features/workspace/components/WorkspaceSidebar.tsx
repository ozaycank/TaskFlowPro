'use client';

import { WorkspaceSwitcher } from './WorkspaceSwitcher';
import { Settings, Users, FolderKanban, Activity } from 'lucide-react';
import Link from 'next/link';
import { useWorkspaceStore } from '../stores/useWorkspaceStore';
import { usePathname } from 'next/navigation';
import { NotificationBell } from '@/features/notifications/components/NotificationBell';
export const WorkspaceSidebar = () => {
  const { activeWorkspaceId } = useWorkspaceStore();
  const pathname = usePathname();

  const getNavLinks = () => {
    if (!activeWorkspaceId) return [];
    const base = `/workspaces/${activeWorkspaceId}`;
    return [
      { name: 'Dashboard', href: base, icon: Activity },
      { name: 'Projects', href: `${base}/projects`, icon: FolderKanban },
      { name: 'Members', href: `${base}/members`, icon: Users },
      { name: 'Settings', href: `${base}/settings`, icon: Settings },
    ];
  };

  const navLinks = getNavLinks();

  return (
    <div className="w-64 border-r border-zinc-200 dark:border-zinc-800 bg-zinc-50/50 dark:bg-zinc-950/50 flex flex-col h-full">
      <div className="p-4 border-b border-zinc-200 dark:border-zinc-800">
        <WorkspaceSwitcher />
        <div className="ml-2">
            <NotificationBell />
        </div>
      </div>
      <nav className="flex-1 p-4 space-y-1">
        {navLinks.map((link) => {
          const Icon = link.icon;
          const isActive = pathname === link.href;
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