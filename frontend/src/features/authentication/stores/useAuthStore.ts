import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';
import { UserDto } from '../types/auth.types';

interface AuthState {
    accessToken: string | null;
    user: UserDto | null;
    isAuthenticated: boolean;
    isInitializing: boolean;
    setAuth: (token: string, user: UserDto) => void;
    clearAuth: () => void;
    setInitializing: (status: boolean) => void;
}

export const useAuthStore = create<AuthState>()(
    persist(
        (set) => ({
            accessToken: null,
            user: null,
            isAuthenticated: false,
            // isInitializing is NOT persisted, it always starts as true on fresh reload
            isInitializing: true,
            setAuth: (token, user) => set({ accessToken: token, user, isAuthenticated: true }),
            clearAuth: () => set({ accessToken: null, user: null, isAuthenticated: false }),
            setInitializing: (status) => set({ isInitializing: status }),
        }),
        {
            name: 'velyo-auth-storage', // The exact key we used in the Axios Interceptor
            storage: createJSONStorage(() => localStorage),
            // We only want to persist the token and user data, NOT the initializing state
            partialize: (state) => ({
                accessToken: state.accessToken,
                user: state.user,
                isAuthenticated: state.isAuthenticated
            }),
        }
    )
);