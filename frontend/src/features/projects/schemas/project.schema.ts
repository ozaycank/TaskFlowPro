import { z } from 'zod';

export const createProjectSchema = z.object({
    name: z.string().min(2, 'Name must be at least 2 characters.').max(50),
    description: z.string().max(500).optional(),
    color: z.string().regex(/^#[0-9A-Fa-f]{6}$/, 'Invalid hex color code.').optional(),
});

export const updateProjectSchema = z.object({
    name: z.string().min(2, 'Name must be at least 2 characters.').max(50),
    description: z.string().max(500).optional(),
    color: z.string().regex(/^#[0-9A-Fa-f]{6}$/, 'Invalid hex color code.').optional(),
    isArchived: z.boolean().optional(),
});

export type CreateProjectFormData = z.infer<typeof createProjectSchema>;
export type UpdateProjectFormData = z.infer<typeof updateProjectSchema>;