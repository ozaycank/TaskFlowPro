import { z } from 'zod';
import { PriorityLevel } from '../types/task.types';

export const createTaskSchema = z.object({
    title: z.string().min(2, 'Title must be at least 2 characters.').max(300),
    description: z.string().max(2000).optional(),
    stateId: z.string().min(1, 'Please select a column (state).'),
    priority: z.nativeEnum(PriorityLevel),
    dueDate: z.string().optional().nullable(),
    parentTaskId: z.string().optional().nullable(),
    labels: z.array(z.string()).optional(),
});

export const updateTaskSchema = z.object({
    title: z.string().min(2, 'Title must be at least 2 characters.').max(300),
    description: z.string().max(2000).optional(),
    priority: z.nativeEnum(PriorityLevel),
    dueDate: z.string().optional().nullable(),
    parentTaskId: z.string().optional().nullable(),
    labels: z.array(z.string()).optional(),
});

export type CreateTaskFormData = z.infer<typeof createTaskSchema>;
export type UpdateTaskFormData = z.infer<typeof updateTaskSchema>;