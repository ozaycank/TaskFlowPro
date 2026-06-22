'use client';

import { useAuthStore } from '@/features/authentication/stores/useAuthStore';
import { useRouter } from 'next/navigation';
import { useEffect } from 'react';

export default function DashboardLayout({ children }: { children: React.ReactNode }) {
  const { isAuthenticated, isInitializing } = useAuthStore();
  const router = useRouter();

  useEffect(() => {
    if (!isInitializing && !isAuthenticated) {
      router.replace('/login');
    }
  }, [isAuthenticated, isInitializing, router]);

  if (!isAuthenticated) return null; // Prevents flash of protected content

  return (
    <div className="min-h-screen flex bg-white dark:bg-zinc-950">
      {/* Sidebar Placeholder */}
      <aside className="w-64 border-r border-zinc-200 dark:border-zinc-800 hidden md:block">
        <div className="p-4 font-bold text-xl dark:text-white">Velyo</div>
      </aside>
      
      {/* Main Content */}
      <main className="flex-1 flex flex-col">
        {/* Topbar Placeholder */}
        <header className="h-14 border-b border-zinc-200 dark:border-zinc-800 flex items-center px-6">
           <span className="text-sm text-zinc-500">Dashboard</span>
        </header>
        
        <div className="p-6">
          {children}
        </div>
      </main>
    </div>
  );
}