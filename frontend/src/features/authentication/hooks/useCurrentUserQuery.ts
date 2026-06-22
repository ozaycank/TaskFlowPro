import { useQuery } from '@tanstack/react-query';
import { authApi } from '../api/auth.api';

export const useCurrentUserQuery = (enabled: boolean = true) => {
    return useQuery({
        queryKey: ['currentUser'],
        queryFn: () => authApi.getCurrentUser(),
        enabled,
        staleTime: 1000 * 60 * 60, // 1 hour
        retry: false,
    });
};