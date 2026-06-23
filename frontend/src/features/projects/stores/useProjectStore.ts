import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';

interface ProjectState {
    activeProjectId: string | null;
    viewMode: 'grid' | 'list';
    projectFilters: {
        showArchived: boolean;
        searchQuery: string;
    };
    setActiveProject: (id: string | null) => void;
    setViewMode: (mode: 'grid' | 'list') => void;
    setFilters: (filters: Partial<ProjectState['projectFilters']>) => void;
    clearState: () => void;
}

export const useProjectStore = create<ProjectState>()(
    persist(
        (set) => ({
            activeProjectId: null,
            viewMode: 'grid',
            projectFilters: {
                showArchived: false,
                searchQuery: '',
            },
            setActiveProject: (id) => set({ activeProjectId: id }),
            setViewMode: (mode) => set({ viewMode: mode }),
            setFilters: (filters) => set((state) => ({
                projectFilters: { ...state.projectFilters, ...filters }
            })),
            clearState: () => set({ activeProjectId: null, projectFilters: { showArchived: false, searchQuery: '' } }),
        }),
        {
            name: 'velyo-project-storage',
            storage: createJSONStorage(() => localStorage),
        }
    )
);