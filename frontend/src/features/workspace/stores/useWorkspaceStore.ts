import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';

interface WorkspaceState {
    activeWorkspaceId: string | null;
    setActiveWorkspace: (id: string) => void;
    clearActiveWorkspace: () => void;
}

export const useWorkspaceStore = create<WorkspaceState>()(
    persist(
        (set) => ({
            activeWorkspaceId: null,
            setActiveWorkspace: (id) => set({ activeWorkspaceId: id }),
            clearActiveWorkspace: () => set({ activeWorkspaceId: null }),
        }),
        {
            name: 'velyo-workspace-storage', // unique name
            storage: createJSONStorage(() => localStorage),
        }
    )
);