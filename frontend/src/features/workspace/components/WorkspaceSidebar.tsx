'use client';

import { useState, useEffect } from 'react'; 
import { WorkspaceSwitcher } from './WorkspaceSwitcher';
import { Settings, Users, FolderKanban, Activity, ListTodo, BarChart2, Book, LayoutDashboard, LogOut, Sun, Moon } from 'lucide-react';
import Link from 'next/link';
import Image from 'next/image'; 
import { useWorkspaceStore } from '../stores/useWorkspaceStore';
import { useAuthStore } from '@/features/authentication/stores/useAuthStore';
import { usePathname, useParams, useRouter } from 'next/navigation';
import { NotificationBell } from '@/features/notifications/components/NotificationBell';
import { GlobalSearchModal } from '@/features/search/components/GlobalSearchModal';
import { useQueryClient } from '@tanstack/react-query';
import { apiClient } from '@/api/client';
import { useTheme } from 'next-themes';

export const WorkspaceSidebar = () => {
  const { activeWorkspaceId } = useWorkspaceStore();
  const { clearAuth, user } = useAuthStore();
  const router = useRouter();
  const pathname = usePathname();
  const queryClient = useQueryClient();
  const params = useParams();
  const projectId = params.projectId as string | undefined;

  const { theme, setTheme } = useTheme();
  const [mounted, setMounted] = useState(false);

  // Next.js Hydration uyumsuzluğunu önlemek için
  useEffect(() => {
    setMounted(true);
  }, []);

  if (!activeWorkspaceId) return null;

  const base = `/workspaces/${activeWorkspaceId}`;

  const workspaceLinks = [
    { name: 'Dashboard', href: base, icon: Activity, exact: true },
    { name: 'Projects', href: `${base}/projects`, icon: FolderKanban, exact: true },
    { name: 'Members', href: `${base}/members`, icon: Users },
    { name: 'Documents', href: `${base}/documents`, icon: Book },
    { name: 'Settings', href: `${base}/settings`, icon: Settings },
  ];

  const projectLinks = projectId ? [
    { 
      name: 'Kanban Board', 
      href: `${base}/projects/${projectId}`, 
      icon: LayoutDashboard,
      matcher: (p: string) => p === `${base}/projects/${projectId}` || p.startsWith(`${base}/projects/${projectId}/tasks`)
    },
    { 
      name: 'Sprints & Backlog', 
      href: `${base}/projects/${projectId}/sprints`, 
      icon: ListTodo 
    },
    { 
      name: 'Analytics', 
      href: `${base}/projects/${projectId}/analytics`, 
      icon: BarChart2 
    }
  ] : [];

  const renderLink = (link: any) => {
    const Icon = link.icon;
    let isActive = false;

    if (link.matcher) {
        isActive = link.matcher(pathname);
    } else if (link.exact) {
        isActive = pathname === link.href;
    } else {
        isActive = pathname.startsWith(link.href);
    }

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
  };

  const handleLogout = async () => {
    try {
        await apiClient.post('/auth/logout');
    } catch (e) { }

    if (typeof document !== 'undefined') {
        document.cookie = 'refreshToken=; Max-Age=0; path=/;';
    }

    clearAuth();
    queryClient.clear();
    
    router.push('/login');
  };

  return (
    <div className="w-64 border-r border-zinc-200 dark:border-zinc-800 bg-zinc-50/50 dark:bg-zinc-950/50 flex flex-col h-full">
      <div className="h-16 flex items-center px-4 border-b border-zinc-200 dark:border-zinc-800">
        <Link href="/workspaces" className="flex items-center gap-3 transition-opacity hover:opacity-80">
            <Image 
                src="/logo.png" 
                alt="Velyo Logo" 
                width={28} 
                height={28} 
                className="rounded-md"
            />
            <span className="text-xl font-bold tracking-tight text-zinc-900 dark:text-white">
                Velyo
            </span>
        </Link>
      </div>

      <div className="p-4 border-b border-zinc-200 dark:border-zinc-800 flex justify-between items-center">
        <div className="flex-1 min-w-0">
          <WorkspaceSwitcher />
        </div>
        <div className="ml-2 flex-shrink-0">
          <NotificationBell />
        </div>
      </div>
      
      <div className="px-4 pt-4 pb-2 w-full overflow-hidden [&>*]:w-full [&_button]:w-full">
        <GlobalSearchModal />
      </div>

      <nav className="flex-1 px-4 py-2 space-y-4 overflow-y-auto">
        <div className="space-y-1">
          <p className="px-3 text-xs font-semibold text-zinc-500 uppercase tracking-wider mb-2 mt-2">
            Workspace
          </p>
          {workspaceLinks.map(renderLink)}
        </div>

        {projectLinks.length > 0 && (
          <div className="space-y-1 pt-2 border-t border-zinc-200 dark:border-zinc-800">
            <p className="px-3 text-xs font-semibold text-zinc-500 uppercase tracking-wider mb-2">
              Current Project
            </p>
            {projectLinks.map(renderLink)}
          </div>
        )}
      </nav>

      <div className="p-4 border-t border-zinc-200 dark:border-zinc-800">
        <div className="flex items-center justify-between">
            <div className="flex flex-col overflow-hidden">
                <span className="text-sm font-medium text-zinc-900 dark:text-white truncate">
                    {user?.firstName} {user?.lastName}
                </span>
                <span className="text-xs text-zinc-500 truncate">{user?.email}</span>
            </div>
            
            <div className="flex items-center gap-1">
                {/* Tema Değiştirme Butonu */}
                {mounted && (
                    <button
                        onClick={() => setTheme(theme === 'dark' ? 'light' : 'dark')}
                        className="p-2 text-zinc-500 hover:text-zinc-900 hover:bg-zinc-200 dark:hover:text-white dark:hover:bg-zinc-800 rounded-md transition-colors"
                        title="Toggle theme"
                    >
                        {theme === 'dark' ? <Sun size={18} /> : <Moon size={18} />}
                    </button>
                )}

                {/* Çıkış Yap Butonu */}
                <button 
                    onClick={handleLogout}
                    className="p-2 text-zinc-500 hover:text-red-600 hover:bg-red-50 dark:hover:bg-red-950/30 rounded-md transition-colors"
                    title="Log out"
                >
                    <LogOut size={18} />
                </button>
            </div>
        </div>
      </div>
    </div>
  );
};