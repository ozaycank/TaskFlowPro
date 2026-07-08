'use client';

import { useFieldDefinitionsQuery, useDeleteFieldDefinitionMutation } from '../hooks/useCustomFieldHooks';
import { CreateCustomFieldDialog } from './CreateCustomFieldDialog';
import { FieldType } from '../types/custom-field.types';
import { Trash2 } from 'lucide-react';
import { Skeleton } from '@/components/ui/skeleton';

export const WorkspaceCustomFieldsSettings = ({ workspaceId }: { workspaceId: string }) => {
    const { data: fields, isLoading } = useFieldDefinitionsQuery(workspaceId);
    const { mutate: deleteField, isPending: isDeleting } = useDeleteFieldDefinitionMutation(workspaceId);

    return (
        <div className="bg-white dark:bg-zinc-900 border border-zinc-200 dark:border-zinc-800 rounded-xl p-6 shadow-sm mt-8">
            <div className="flex justify-between items-center mb-6">
                <div>
                    <h2 className="text-xl font-semibold dark:text-zinc-100">Custom Fields</h2>
                    <p className="text-sm text-zinc-500">Define dynamic properties for tasks across this workspace.</p>
                </div>
                <CreateCustomFieldDialog workspaceId={workspaceId} />
            </div>

            {isLoading ? (
                <div className="space-y-2"><Skeleton className="h-12 w-full"/><Skeleton className="h-12 w-full"/></div>
            ) : fields?.length === 0 ? (
                <div className="p-8 text-center text-zinc-500 border border-dashed rounded-lg dark:border-zinc-800">
                    No custom fields defined yet.
                </div>
            ) : (
                <div className="border border-zinc-200 dark:border-zinc-800 rounded-lg overflow-hidden">
                    <table className="w-full text-sm text-left">
                        <thead className="bg-zinc-50 dark:bg-zinc-950/50 text-zinc-500 dark:text-zinc-400">
                            <tr>
                                <th className="px-4 py-3 font-medium">Field Name</th>
                                <th className="px-4 py-3 font-medium">Type</th>
                                <th className="px-4 py-3 font-medium">Required</th>
                                <th className="px-4 py-3 text-right font-medium">Actions</th>
                            </tr>
                        </thead>
                        <tbody className="divide-y divide-zinc-200 dark:divide-zinc-800">
                            {fields?.map(field => (
                                <tr key={field.id} className="hover:bg-zinc-50 dark:hover:bg-zinc-900/50">
                                    <td className="px-4 py-3 font-medium dark:text-zinc-200">{field.name}</td>
                                    <td className="px-4 py-3 text-zinc-500">{FieldType[field.type]}</td>
                                    <td className="px-4 py-3 text-zinc-500">{field.isRequired ? 'Yes' : 'No'}</td>
                                    <td className="px-4 py-3 text-right">
                                        <button 
                                            onClick={() => deleteField(field.id)}
                                            disabled={isDeleting}
                                            className="text-zinc-400 hover:text-red-500 transition-colors disabled:opacity-50"
                                        >
                                            <Trash2 size={16} />
                                        </button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            )}
        </div>
    );
};