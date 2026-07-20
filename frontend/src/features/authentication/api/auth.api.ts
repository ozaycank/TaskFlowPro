import { apiClient } from '@/api/client';
import { LoginResponseDto, UserDto, RegisterRequest } from '../types/auth.types';
import { LoginFormData } from '../schemas/auth.schema';

export const authApi = {
    register: async (request: RegisterRequest) => {
        // Backend'deki dönüş tipi Login ile aynı (AuthResponseDto)
        const { data } = await apiClient.post('/auth/register', request);
        return data;
    },

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