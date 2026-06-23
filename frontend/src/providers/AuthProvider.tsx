'use client';

import { useEffect } from 'react';
import { useAuthStore } from '@/features/authentication/stores/useAuthStore';
import { authApi } from '@/features/authentication/api/auth.api';

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const { setAuth, clearAuth, setInitializing, isInitializing, isAuthenticated } = useAuthStore();

  useEffect(() => {
    const initializeAuth = async () => {
      // Eğer zaten Zustand store'da (hafızada) login olduysak, 
      // her F5'te zorla refresh atmayı es geçebiliriz (Backend tam hazır olana kadar)
      if (isAuthenticated) {
         setInitializing(false);
         return;
      }

      try {
        // Attempt silent refresh on startup to recover session
        const data = await authApi.refresh();
        setAuth(data.accessToken, {
          id: data.userId,
          email: data.email,
          firstName: data.firstName,
          lastName: data.lastName,
          isActive: true
        });
      } catch (error) {
        // Sessizce yutuyoruz, zorla logout veya redirect YAPMIYORUZ.
        // Sadece state'i temizliyoruz.
        clearAuth();
      } finally {
        setInitializing(false);
      }
    };

    initializeAuth();
  }, [setAuth, clearAuth, setInitializing, isAuthenticated]);

  if (isInitializing) {
    return <div className="min-h-screen flex items-center justify-center bg-zinc-50 dark:bg-zinc-950">Loading Velyo...</div>;
  }

  return <>{children}</>;
};