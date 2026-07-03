'use client';

import { VelocityChart } from './VelocityChart';
import { BurndownChart } from './BurndownChart';
import { CycleTimeChart } from './CycleTimeChart';
import { CumulativeFlowChart } from './CumulativeFlowChart';
import { AnalyticsKpiCards } from './AnalyticsKpiCards';
import { useSprintsQuery } from '@/features/sprints/hooks/useSprintsQuery';
import { SprintStatus } from '@/features/sprints/types/sprint.types';

export const AnalyticsDashboard = ({ projectId }: { projectId: string }) => {
    const { data: sprints } = useSprintsQuery(projectId);
    const activeSprint = sprints?.find(s => s.status === SprintStatus.Active);

    return (
        <div className="space-y-6 max-w-7xl mx-auto pb-12 w-full">
            <div>
                <h2 className="text-2xl font-bold tracking-tight dark:text-zinc-50">Project Analytics</h2>
                <p className="text-zinc-500">Monitor team performance, velocity, and delivery health.</p>
            </div>

            {/* KPI Summary Row */}
            <AnalyticsKpiCards projectId={projectId} />

            <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
                {/* VELOCITY */}
                <div className="bg-white dark:bg-zinc-900 border border-zinc-200 dark:border-zinc-800 rounded-xl p-6 shadow-sm">
                    <div className="mb-4">
                        <h3 className="font-semibold text-lg dark:text-zinc-100">Sprint Velocity</h3>
                        <p className="text-xs text-zinc-500">Committed vs Completed tasks over time.</p>
                    </div>
                    <VelocityChart projectId={projectId} />
                </div>

                {/* BURNDOWN */}
                <div className="bg-white dark:bg-zinc-900 border border-zinc-200 dark:border-zinc-800 rounded-xl p-6 shadow-sm">
                    <div className="mb-4 flex justify-between items-center">
                        <div>
                            <h3 className="font-semibold text-lg dark:text-zinc-100">Active Sprint Burndown</h3>
                            <p className="text-xs text-zinc-500">Daily progress tracking against ideal trend.</p>
                        </div>
                        {activeSprint && (
                            <span className="text-xs font-medium px-2 py-1 bg-indigo-100 text-indigo-700 dark:bg-indigo-900/30 dark:text-indigo-400 rounded-full">
                                {activeSprint.name}
                            </span>
                        )}
                    </div>
                    <BurndownChart sprintId={activeSprint?.id} />
                </div>

                {/* CUMULATIVE FLOW DIAGRAM */}
                <div className="bg-white dark:bg-zinc-900 border border-zinc-200 dark:border-zinc-800 rounded-xl p-6 shadow-sm lg:col-span-2">
                    <div className="mb-4">
                        <h3 className="font-semibold text-lg dark:text-zinc-100">Cumulative Flow Diagram (CFD)</h3>
                        <p className="text-xs text-zinc-500">Work item stability and bottleneck identification across workflow states.</p>
                    </div>
                    <CumulativeFlowChart projectId={projectId} />
                </div>

                {/* CYCLE TIME SCATTER */}
                <div className="bg-white dark:bg-zinc-900 border border-zinc-200 dark:border-zinc-800 rounded-xl p-6 shadow-sm lg:col-span-2">
                    <div className="mb-4">
                        <h3 className="font-semibold text-lg dark:text-zinc-100">Cycle Time Distribution</h3>
                        <p className="text-xs text-zinc-500">Identify outlier tasks and delivery consistency.</p>
                    </div>
                    <CycleTimeChart projectId={projectId} />
                </div>
            </div>
        </div>
    );
};