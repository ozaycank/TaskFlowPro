'use client';

import { useEffect } from 'react';
import { useAuthStore } from '@/features/authentication/stores/useAuthStore';
import { authApi } from '@/features/authentication/api/auth.api';

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const { setAuth, clearAuth, setInitializing, isInitializing } = useAuthStore();

  useEffect(() => {
    const initializeAuth = async () => {
      try {
        // Attempt silent refresh on startup to recover session
        const { accessToken, user } = await authApi.refresh();
        setAuth(accessToken, user);
      } catch (error) {
        // No valid session, clear state
        clearAuth();
      } finally {
        setInitializing(false);
      }
    };

    initializeAuth();
  }, [setAuth, clearAuth, setInitializing]);

  if (isInitializing) {
    return <div className="min-h-screen flex items-center justify-center bg-zinc-50 dark:bg-zinc-950">Loading Velyo...</div>;
  }

  return <>{children}</>;
};