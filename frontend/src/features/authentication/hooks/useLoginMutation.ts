import { useMutation } from '@tanstack/react-query';
import { authApi } from '../api/auth.api';
import { useAuthStore } from '../stores/useAuthStore';
import { LoginFormData } from '../schemas/auth.schema';
import { AxiosError } from 'axios';
import { ProblemDetails } from '../types/auth.types';

export const useLoginMutation = () => {
    const setAuth = useAuthStore((state) => state.setAuth);

    return useMutation<Awaited<ReturnType<typeof authApi.login>>, AxiosError<ProblemDetails>, LoginFormData>({
        mutationFn: (credentials) => authApi.login(credentials),
        onSuccess: (data) => {
            setAuth(data.accessToken, data.user);
        },
    });
};