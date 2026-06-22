import { useMutation, useQueryClient } from '@tanstack/react-query';
import { authApi } from '../api/auth.api';
import { useAuthStore } from '../stores/useAuthStore';
import { useRouter } from 'next/navigation';

export const useLogoutMutation = () => {
    const clearAuth = useAuthStore((state) => state.clearAuth);
    const queryClient = useQueryClient();
    const router = useRouter();

    return useMutation({
        mutationFn: () => authApi.logout(),
        onSettled: () => {
            // Regardless of success or failure, we clear local state
            clearAuth();
            queryClient.clear(); // Clear all cached data
            router.push('/login');
        },
    });
};