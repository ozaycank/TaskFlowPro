import { create } from 'zustand';
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

export const useAuthStore = create<AuthState>((set) => ({
    accessToken: null,
    user: null,
    isAuthenticated: false,
    isInitializing: true, // Uygulama ilk açıldığında refresh token kontrolü yapılana kadar true kalır
    setAuth: (token, user) => set({ accessToken: token, user, isAuthenticated: true }),
    clearAuth: () => set({ accessToken: null, user: null, isAuthenticated: false }),
    setInitializing: (status) => set({ isInitializing: status }),
}));