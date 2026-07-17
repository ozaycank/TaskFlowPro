import { apiClient } from '@/api/client';
import { SearchResultDto } from '../types/search.types';

export const searchApi = {
    globalSearch: async (workspaceId: string, query: string): Promise<SearchResultDto[]> => {
        if (!query.trim()) return [];

        const response = await apiClient.get<SearchResultDto[]>(`/search`, {
            params: {
                workspaceId,
                q: query
            }
        });
        return response.data;
    }
};