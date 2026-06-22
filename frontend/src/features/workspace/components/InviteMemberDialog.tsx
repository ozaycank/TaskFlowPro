'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { inviteMemberSchema, InviteMemberFormData } from '../schemas/workspace.schema';
import { useInviteMemberMutation } from '../hooks/useInviteMemberMutation';
import { WorkspaceRole } from '../types/workspace.types';
import { UserPlus } from 'lucide-react';

export const InviteMemberDialog = ({ workspaceId }: { workspaceId: string }) => {
  const [open, setOpen] = useState(false);
  const { mutate, isPending } = useInviteMemberMutation(workspaceId);

  const { register, handleSubmit, setValue, formState: { errors }, reset } = useForm<InviteMemberFormData>({
    resolver: zodResolver(inviteMemberSchema),
    defaultValues: { role: WorkspaceRole.Member }
  });

  const onSubmit = (data: InviteMemberFormData) => {
    mutate(data, {
      onSuccess: () => {
        setOpen(false);
        reset();
      },
    });
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button>
          <UserPlus className="mr-2 h-4 w-4" /> Invite Member
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px]">
        <DialogHeader>
          <DialogTitle>Invite to Workspace</DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4 pt-4">
          <div className="space-y-2">
            <Label>Email Address</Label>
            <Input placeholder="colleague@company.com" {...register('email')} />
            {errors.email && <span className="text-xs text-red-500">{errors.email.message}</span>}
          </div>
          <div className="space-y-2">
            <Label>Role</Label>
            <Select onValueChange={(val) => setValue('role', val as WorkspaceRole)} defaultValue={WorkspaceRole.Member}>
              <SelectTrigger>
                <SelectValue placeholder="Select a role" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value={WorkspaceRole.Admin}>Admin</SelectItem>
                <SelectItem value={WorkspaceRole.Member}>Member</SelectItem>
                <SelectItem value={WorkspaceRole.Guest}>Guest</SelectItem>
              </SelectContent>
            </Select>
            {errors.role && <span className="text-xs text-red-500">{errors.role.message}</span>}
          </div>
          <Button type="submit" disabled={isPending} className="w-full">
            {isPending ? 'Sending Invite...' : 'Send Invitation'}
          </Button>
        </form>
      </DialogContent>
    </Dialog>
  );
};