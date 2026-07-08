'use client';

import { useState, useEffect } from 'react';
import { CustomFieldDefinitionDto, FieldType } from '../types/custom-field.types';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';

interface Props {
    definition: CustomFieldDefinitionDto;
    value: string;
    onSave: (value: string) => void;
    isPending: boolean;
}

export const DynamicFieldRenderer = ({ definition, value, onSave, isPending }: Props) => {
    const [localValue, setLocalValue] = useState(value || '');

    useEffect(() => {
        setLocalValue(value || '');
    }, [value]);

    const handleBlur = () => {
        if (localValue !== value) {
            onSave(localValue);
        }
    };

    const renderInput = () => {
        switch (definition.type) {
            case FieldType.Boolean:
                return (
                    <input 
                        type="checkbox" 
                        className="w-4 h-4 rounded border-zinc-300 text-indigo-600 focus:ring-indigo-500"
                        checked={localValue === 'true'}
                        onChange={(e) => {
                            const val = e.target.checked ? 'true' : 'false';
                            setLocalValue(val);
                            onSave(val);
                        }}
                        disabled={isPending}
                    />
                );
            case FieldType.SingleSelect:
                const options: string[] = definition.optionsJson ? JSON.parse(definition.optionsJson) : [];
                return (
                    <select 
                        className="flex h-9 w-full rounded-md border border-zinc-200 bg-transparent px-3 py-1 text-sm shadow-sm transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-zinc-950 disabled:cursor-not-allowed disabled:opacity-50 dark:border-zinc-800 dark:focus-visible:ring-zinc-300"
                        value={localValue}
                        onChange={(e) => {
                            setLocalValue(e.target.value);
                            onSave(e.target.value);
                        }}
                        disabled={isPending}
                    >
                        <option value="">Select...</option>
                        {options.map(opt => <option key={opt} value={opt}>{opt}</option>)}
                    </select>
                );
            case FieldType.Date:
                return (
                    <Input 
                        type="date" 
                        value={localValue} 
                        onChange={(e) => setLocalValue(e.target.value)}
                        onBlur={handleBlur}
                        disabled={isPending}
                    />
                );
            case FieldType.Number:
                return (
                    <Input 
                        type="number" 
                        value={localValue} 
                        onChange={(e) => setLocalValue(e.target.value)}
                        onBlur={handleBlur}
                        disabled={isPending}
                    />
                );
            case FieldType.Url:
            case FieldType.Text:
            default:
                return (
                    <Input 
                        type="text" 
                        value={localValue} 
                        onChange={(e) => setLocalValue(e.target.value)}
                        onBlur={handleBlur}
                        disabled={isPending}
                        placeholder={`Enter ${definition.name.toLowerCase()}...`}
                    />
                );
        }
    };

    return (
        <div className="space-y-1.5">
            <Label className="text-xs font-semibold text-zinc-500 dark:text-zinc-400 flex justify-between">
                <span>{definition.name} {definition.isRequired && <span className="text-red-500">*</span>}</span>
                {isPending && <span className="text-xs text-indigo-500">Saving...</span>}
            </Label>
            <div className="flex items-center gap-2">
                {renderInput()}
            </div>
        </div>
    );
};