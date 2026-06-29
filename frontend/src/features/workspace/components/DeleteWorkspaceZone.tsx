'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { useDeleteWorkspaceMutation } from '../hooks/useWorkspaceSettings';
import { useWorkspaceStore } from '../stores/useWorkspaceStore';
import { AlertTriangle } from 'lucide-react';
import { WorkspaceDetailDto } from '../api/workspace-settings.api';

interface Props {
    workspace: WorkspaceDetailDto;
}

export const DeleteWorkspaceZone = ({ workspace }: Props) => {
    const [confirmName, setConfirmName] = useState('');
    const { mutate: deleteWorkspace, isPending } = useDeleteWorkspaceMutation();
    const router = useRouter();
    
    // FIXED: setActiveWorkspaceId yerine deponuzdaki doğru metot olan clearActiveWorkspace kullanıldı.
    const { clearActiveWorkspace } = useWorkspaceStore();

    const isMatch = confirmName === workspace.name;

    const handleDelete = () => {
        if (!isMatch) return;
        
        deleteWorkspace(workspace.id, {
            onSuccess: () => {
                // FIXED: Silme başarılı olduğunda state'i temizleyen doğru fonksiyon çağrıldı.
                clearActiveWorkspace();
                router.push('/workspaces'); // Projelerin listelendiği ana giriş sayfası
            }
        });
    };

    return (
        <div className="border border-red-200 dark:border-red-900/50 rounded-xl p-6 bg-red-50/50 dark:bg-red-950/10 mt-8">
            <div className="flex items-start gap-4">
                <div className="p-2 bg-red-100 dark:bg-red-900/30 rounded-full text-red-600 dark:text-red-400">
                    <AlertTriangle size={24} />
                </div>
                <div className="flex-1">
                    <h3 className="text-lg font-semibold text-red-600 dark:text-red-400">Danger Zone</h3>
                    <p className="text-sm text-zinc-600 dark:text-zinc-400 mt-1 mb-4">
                        Deleting this workspace will permanently remove all associated projects, workflows, sprints, and tasks. This action <strong>cannot be undone</strong>.
                    </p>

                    <div className="space-y-2 max-w-md">
                        <Label className="text-red-600 dark:text-red-400">
                            Please type <strong>{workspace.name}</strong> to confirm.
                        </Label>
                        <Input 
                            value={confirmName} 
                            onChange={(e) => setConfirmName(e.target.value)} 
                            placeholder={workspace.name}
                            className="border-red-200 focus-visible:ring-red-500"
                        />
                    </div>

                    <Button 
                        variant="destructive" 
                        className="mt-4" 
                        disabled={!isMatch || isPending}
                        onClick={handleDelete}
                    >
                        {isPending ? 'Deleting...' : 'Permanently Delete Workspace'}
                    </Button>
                </div>
            </div>
        </div>
    );
};