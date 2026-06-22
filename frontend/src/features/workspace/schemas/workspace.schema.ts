import { z } from 'zod';
import { WorkspaceRole } from '../types/workspace.types';

export const createWorkspaceSchema = z.object({
    name: z.string().min(3, 'Workspace name must be at least 3 characters.').max(50),
    description: z.string().max(200).optional(),
});

export const inviteMemberSchema = z.object({
    email: z.string().email('Invalid email address.'),
    // FIXED: Following exact strict type signature of Zod's nativeEnum options object.
    // The compiler explicitly expects either 'error' or 'message'.
    role: z.nativeEnum(WorkspaceRole, {
        message: 'Please select a valid role.',
    }),
});

export type CreateWorkspaceFormData = z.infer<typeof createWorkspaceSchema>;
export type InviteMemberFormData = z.infer<typeof inviteMemberSchema>;