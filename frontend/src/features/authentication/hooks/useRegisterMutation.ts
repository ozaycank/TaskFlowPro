import { useMutation } from '@tanstack/react-query';
import { authApi } from '../api/auth.api';
import { RegisterRequest } from '../types/auth.types';
import { useAuthStore } from '../stores/useAuthStore';
import { useRouter } from 'next/navigation';

export const useRegisterMutation = () => {
    const router = useRouter();
    const setAuth = useAuthStore((state) => state.setAuth);

    return useMutation({
        mutationFn: (data: RegisterRequest) => authApi.register(data),
        onSuccess: (data: any) => {
            // Backend'den dönen AuthResponseDto içindeki token alanını alıyoruz
            const token = data.accessToken;

            // Store'daki UserDto yapısına uygun şekilde kullanıcı objesini oluşturuyoruz
            const user = {
                id: data.id,
                email: data.email,
                firstName: data.firstName,
                lastName: data.lastName,
                isActive: data.isActive,
            };

            // Store'un beklediği 2 parametreyi (token ve user) gönderiyoruz
            setAuth(token, user);

            // Başarılı kayıttan sonra workspace seçimine/listesine yönlendir
            router.push('/workspaces');
        },
    });
};