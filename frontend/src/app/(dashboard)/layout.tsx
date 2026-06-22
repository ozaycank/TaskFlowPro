'use client';

import { useAuthStore } from '@/features/authentication/stores/useAuthStore';
import { WorkspaceSidebar } from '@/features/workspace/components/WorkspaceSidebar';
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

  if (!isAuthenticated) return null;

  return (
    <div className="h-screen w-full flex bg-white dark:bg-zinc-950 overflow-hidden">
      {/* ENTERPRISE WORKSPACE SIDEBAR INJECTED HERE */}
      <WorkspaceSidebar />
      
      <main className="flex-1 flex flex-col overflow-auto">
        <div className="flex-1 p-8">
          {children}
        </div>
      </main>
    </div>
  );
}