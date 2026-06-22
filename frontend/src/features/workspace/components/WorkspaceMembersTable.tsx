'use client';

import { useWorkspaceMembersQuery } from '../hooks/useWorkspaceMembersQuery';
import { useRemoveMemberMutation } from '../hooks/useRemoveMemberMutation';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table';
import { Button } from '@/components/ui/button';
import { Avatar, AvatarFallback } from '@/components/ui/avatar';
import { Badge } from '@/components/ui/badge';
import { Trash2 } from 'lucide-react';
import { WorkspaceRole } from '../types/workspace.types';

export const WorkspaceMembersTable = ({ workspaceId }: { workspaceId: string }) => {
  const { data: members, isLoading } = useWorkspaceMembersQuery(workspaceId);
  const { mutate: removeMember, isPending: isRemoving } = useRemoveMemberMutation(workspaceId);

  if (isLoading) return <div className="animate-pulse h-64 bg-zinc-100 dark:bg-zinc-900 rounded-md" />;
  if (!members?.length) return <div className="text-center py-10 text-zinc-500">No members found.</div>;

  const handleRemove = (memberId: string) => {
    if (confirm('Are you sure you want to remove this member?')) {
      removeMember(memberId);
    }
  };

  return (
    <div className="border border-zinc-200 dark:border-zinc-800 rounded-md">
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>User</TableHead>
            <TableHead>Role</TableHead>
            <TableHead>Joined Date</TableHead>
            <TableHead className="text-right">Actions</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {members.map((member) => (
            <TableRow key={member.id}>
              <TableCell>
                <div className="flex items-center gap-3">
                  <Avatar className="h-8 w-8">
                    <AvatarFallback>{member.firstName[0]}{member.lastName[0]}</AvatarFallback>
                  </Avatar>
                  <div>
                    <div className="font-medium text-sm">{member.firstName} {member.lastName}</div>
                    <div className="text-xs text-zinc-500">{member.email}</div>
                  </div>
                </div>
              </TableCell>
              <TableCell>
                <Badge variant={member.role === WorkspaceRole.Owner ? 'default' : 'secondary'}>
                  {member.role}
                </Badge>
              </TableCell>
              <TableCell className="text-sm text-zinc-500">
                {new Date(member.joinedAt).toLocaleDateString()}
              </TableCell>
              <TableCell className="text-right">
                <Button 
                  variant="ghost" 
                  size="icon" 
                  onClick={() => handleRemove(member.id)}
                  disabled={isRemoving || member.role === WorkspaceRole.Owner}
                  className="text-red-500 hover:text-red-600 hover:bg-red-50 dark:hover:bg-red-900/20"
                >
                  <Trash2 size={16} />
                </Button>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </div>
  );
};