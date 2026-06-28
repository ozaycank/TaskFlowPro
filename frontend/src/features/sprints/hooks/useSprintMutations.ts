import { useMutation, useQueryClient } from '@tanstack/react-query';
import { sprintApi } from '../api/sprint.api';
import { CreateSprintCommand, UpdateSprintCommand } from '../types/sprint.types';
import { SPRINT_QUERY_KEYS } from './useSprintsQuery';

export const useSprintMutations = (projectId: string) => {
    const queryClient = useQueryClient();

    const onSuccess = () => {
        queryClient.invalidateQueries({ queryKey: SPRINT_QUERY_KEYS.byProject(projectId) });
    };

    const createMutation = useMutation({
        mutationFn: (command: CreateSprintCommand) => sprintApi.createSprint(command),
        onSuccess,
    });

    const updateMutation = useMutation({
        mutationFn: (command: UpdateSprintCommand) => sprintApi.updateSprint(command),
        onSuccess,
    });

    const startMutation = useMutation({
        mutationFn: (sprintId: string) => sprintApi.startSprint(sprintId),
        onSuccess,
    });

    const completeMutation = useMutation({
        mutationFn: (sprintId: string) => sprintApi.completeSprint(sprintId),
        onSuccess,
    });

    return { createMutation, updateMutation, startMutation, completeMutation };
};