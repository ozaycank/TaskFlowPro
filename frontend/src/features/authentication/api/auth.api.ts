import { apiClient } from '@/api/client';
import { LoginResponseDto, UserDto } from '../types/auth.types';
import { LoginFormData } from '../schemas/auth.schema';

export const authApi = {
    login: async (credentials: LoginFormData): Promise<LoginResponseDto> => {
        const { data } = await apiClient.post<LoginResponseDto>('/auth/login', credentials);
        return data;
    },

    logout: async (): Promise<void> => {
        await apiClient.post('/auth/logout');
    },

    refresh: async (): Promise<LoginResponseDto> => {
        const { data } = await apiClient.post<LoginResponseDto>('/auth/refresh');
        return data;
    },

    getCurrentUser: async (): Promise<UserDto> => {
        const { data } = await apiClient.get<UserDto>('/auth/me');
        return data;
    },
};