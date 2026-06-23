import { useMutation } from '@tanstack/react-query';
import { authApi } from '../api/auth.api';
import { useAuthStore } from '../stores/useAuthStore';
import { useRouter } from 'next/navigation';
import { LoginResponseDto } from '../types/auth.types';

export const useLoginMutation = () => {
    const setAuth = useAuthStore((state) => state.setAuth);
    const router = useRouter();

    // Type definition restored for maximum safety
    return useMutation({
        mutationFn: (credentials: any) => authApi.login(credentials),
        onSuccess: (data: LoginResponseDto) => {
            // 1. Map flat backend response to structured frontend UserDto
            setAuth(data.accessToken, {
                id: data.userId,
                email: data.email,
                firstName: data.firstName,
                lastName: data.lastName,
                isActive: true // Backend currently doesn't send this, defaulting to true
            });

            // 2. Write refresh token to cookie for Next.js Edge Middleware (Proxy) to read
            document.cookie = `refreshToken=${data.refreshToken}; path=/; max-age=604800; samesite=lax`;

            // 3. Redirect to Workspaces dashboard
            router.push('/workspaces');
        },
    });
};