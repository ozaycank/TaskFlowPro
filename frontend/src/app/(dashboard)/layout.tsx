'use client';

import { WorkspaceSidebar } from '@/features/workspace/components/WorkspaceSidebar';

export default function DashboardLayout({ children }: { children: React.ReactNode }) {
  /* * Strict route protection is temporarily handled by proxy.ts (cookie check).
   * Infinite loop fix is active.
   */

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