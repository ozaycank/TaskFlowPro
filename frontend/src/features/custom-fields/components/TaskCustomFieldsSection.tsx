'use client';

import { useFieldDefinitionsQuery, useSetTaskFieldValueMutation } from '../hooks/useCustomFieldHooks';
import { DynamicFieldRenderer } from './DynamicFieldRenderer';
import { Skeleton } from '@/components/ui/skeleton';

interface Props {
    taskId: string;
    workspaceId: string;
    projectId: string;
    customFieldsData: Record<string, string>;
}

export const TaskCustomFieldsSection = ({ taskId, workspaceId, projectId, customFieldsData }: Props) => {
    const { data: definitions, isLoading } = useFieldDefinitionsQuery(workspaceId, projectId);
    const { mutate: setValue, isPending } = useSetTaskFieldValueMutation(taskId);

    if (isLoading) return <div className="space-y-4"><Skeleton className="h-10 w-full"/><Skeleton className="h-10 w-full"/></div>;
    if (!definitions || definitions.length === 0) return null; // Don't render section if no fields exist

    return (
        <div className="bg-white dark:bg-zinc-900 border border-zinc-200 dark:border-zinc-800 rounded-xl shadow-sm p-6 mb-6">
            <h3 className="font-semibold dark:text-white mb-4">Properties</h3>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                {definitions.map(def => (
                    <DynamicFieldRenderer 
                        key={def.id}
                        definition={def}
                        value={customFieldsData?.[def.id] || ''}
                        onSave={(newValue) => {
                            setValue({ taskId, fieldDefinitionId: def.id, value: newValue });
                        }}
                        isPending={isPending}
                    />
                ))}
            </div>
        </div>
    );
};