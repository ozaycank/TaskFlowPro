'use client';

import { WorkspaceSwitcher } from './WorkspaceSwitcher';
import { Settings, Users, FolderKanban, Activity, ListTodo, BarChart2, Book, LayoutDashboard } from 'lucide-react';
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

  if (!activeWorkspaceId) return null;

  const base = `/workspaces/${activeWorkspaceId}`;

  // 1. WORKSPACE LEVEL LINKS (Çalışma Alanı Menüleri)
  const workspaceLinks = [
    // EXACT: true ile sadece bu sayfadayken yanmasını (hover) sağlıyoruz.
    { name: 'Dashboard', href: base, icon: Activity, exact: true },
    { name: 'Projects', href: `${base}/projects`, icon: FolderKanban, exact: true },
    { name: 'Members', href: `${base}/members`, icon: Users },
    { name: 'Documents', href: `${base}/documents`, icon: Book },
    { name: 'Settings', href: `${base}/settings`, icon: Settings },
  ];

  // 2. PROJECT LEVEL LINKS (Proje Menüleri - Sadece proje içindeysek görünür)
  const projectLinks = projectId ? [
    { 
      name: 'Kanban Board', 
      href: `${base}/projects/${projectId}`, 
      icon: LayoutDashboard, // İkon Kanban ile karışmasın diye Board ikonuna çevrildi
      // MATCHER: Kanban Board veya içindeki bir Task'a girildiyse aktif kalmasını sağlıyoruz
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

  // Dinamik Link Render Fonksiyonu
  const renderLink = (link: any) => {
    const Icon = link.icon;
    let isActive = false;

    // Aktif sayfa hesaplama mantığı (Çakışmaları önler)
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
      
      {/* 
          FIX: Arama Barı Taşması Çözüldü 
          [&>*]:w-full ve overflow-hidden ile içindeki bileşenin dışarı taşması engellendi.
      */}
      <div className="px-4 pt-4 pb-2 w-full overflow-hidden [&>*]:w-full [&_button]:w-full">
        <GlobalSearchModal />
      </div>

      <nav className="flex-1 px-4 py-2 space-y-4 overflow-y-auto">
        
        {/* BÖLÜM 1: WORKSPACE */}
        <div className="space-y-1">
          <p className="px-3 text-xs font-semibold text-zinc-500 uppercase tracking-wider mb-2 mt-2">
            Workspace
          </p>
          {workspaceLinks.map(renderLink)}
        </div>

        {/* BÖLÜM 2: CURRENT PROJECT (Eğer bir projedeysek görünür) */}
        {projectLinks.length > 0 && (
          <div className="space-y-1 pt-2 border-t border-zinc-200 dark:border-zinc-800">
            <p className="px-3 text-xs font-semibold text-zinc-500 uppercase tracking-wider mb-2">
              Current Project
            </p>
            {projectLinks.map(renderLink)}
          </div>
        )}

      </nav>
    </div>
  );
};