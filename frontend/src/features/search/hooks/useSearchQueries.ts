import { useQuery } from '@tanstack/react-query';
import { searchApi } from '../api/search.api';

export const useGlobalSearchQuery = (workspaceId: string, searchTerm: string) => {
    return useQuery({
        queryKey: ['global-search', workspaceId, searchTerm],
        queryFn: () => searchApi.globalSearch(workspaceId, searchTerm),
        // Sadece arama terimi 2 karakterden uzunsa API isteği at
        enabled: !!workspaceId && searchTerm.trim().length > 1,
        staleTime: 1000 * 60 * 5, // Cache search results for 5 minutes
    });
};