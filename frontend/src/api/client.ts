import axios, { InternalAxiosRequestConfig, AxiosError } from 'axios';
import { useAuthStore } from '@/features/authentication/stores/useAuthStore';
import { authApi } from '@/features/authentication/api/auth.api';

export const apiClient = axios.create({
    baseURL: process.env.NEXT_PUBLIC_API_URL || 'http://127.0.0.1:5137/api',
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    },
});

apiClient.interceptors.request.use((config: InternalAxiosRequestConfig) => {
    // Sadece Zustand'ın güncel durumunu kullanırız.
    // Zustand persist zaten sayfayı yenilediğinizde (hydrate) bu state'i doldurur.
    const token = useAuthStore.getState().accessToken;

    if (token) {
        // Axios config.headers undefined olabileceği için güvenli atama yapıyoruz
        config.headers.set('Authorization', `Bearer ${token}`);
    }

    return config;
});

let isRefreshing = false;
let failedQueue: Array<{
    resolve: (value?: unknown) => void;
    reject: (reason?: any) => void;
}> = [];

const processQueue = (error: any, token: string | null = null) => {
    failedQueue.forEach((prom) => {
        if (error) {
            prom.reject(error);
        } else {
            prom.resolve(token);
        }
    });
    failedQueue = [];
};

apiClient.interceptors.response.use(
    (response) => response,
    async (error: AxiosError) => {
        const originalRequest = error.config as InternalAxiosRequestConfig & { _retry?: boolean };

        if (!originalRequest || originalRequest.url?.includes('/auth/login') || originalRequest.url?.includes('/auth/refresh')) {
            return Promise.reject(error);
        }

        if (error.response?.status === 401 && !originalRequest._retry) {
            if (isRefreshing) {
                return new Promise(function (resolve, reject) {
                    failedQueue.push({ resolve, reject });
                })
                    .then((token) => {
                        originalRequest.headers.set('Authorization', `Bearer ${token}`);
                        return apiClient(originalRequest);
                    })
                    .catch((err) => {
                        return Promise.reject(err);
                    });
            }

            originalRequest._retry = true;
            isRefreshing = true;

            try {
                const data = await authApi.refresh();

                useAuthStore.getState().setAuth(data.accessToken, {
                    id: data.userId,
                    email: data.email,
                    firstName: data.firstName,
                    lastName: data.lastName,
                    isActive: true
                });

                if (typeof document !== 'undefined') {
                    document.cookie = `refreshToken=${data.refreshToken}; path=/; max-age=604800; samesite=lax`;
                }

                processQueue(null, data.accessToken);
                originalRequest.headers.set('Authorization', `Bearer ${data.accessToken}`);

                return apiClient(originalRequest);
            } catch (err) {
                processQueue(err, null);

                if (typeof document !== 'undefined') {
                    document.cookie = 'refreshToken=; Max-Age=0; path=/;';
                }
                useAuthStore.getState().clearAuth();

                if (typeof window !== 'undefined' && !window.location.pathname.includes('/login')) {
                    window.location.href = `/login?callbackUrl=${encodeURIComponent(window.location.pathname)}`;
                }

                return Promise.reject(err);
            } finally {
                isRefreshing = false;
            }
        }

        return Promise.reject(error);
    }
);