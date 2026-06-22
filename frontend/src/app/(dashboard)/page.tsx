'use client';

import { useAuthStore } from '@/features/authentication/stores/useAuthStore';
import { useLogoutMutation } from '@/features/authentication/hooks/useLogoutMutation';

export default function DashboardPage() {
  const { user } = useAuthStore();
  const logoutMutation = useLogoutMutation();

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-3xl font-bold dark:text-white">Welcome, {user?.firstName}</h1>
        <p className="text-zinc-500 mt-2">Here is what is happening in your workspace today.</p>
      </div>

      <button
        onClick={() => logoutMutation.mutate()}
        disabled={logoutMutation.isPending}
        className="px-4 py-2 text-sm text-white bg-red-600 rounded-md hover:bg-red-700"
      >
        {logoutMutation.isPending ? 'Signing out...' : 'Sign Out'}
      </button>
    </div>
  );
}