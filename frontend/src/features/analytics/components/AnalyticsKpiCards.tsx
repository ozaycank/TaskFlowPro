'use client';

import { useVelocityQuery, useCycleTimeQuery } from '../hooks/useAnalyticsHooks';
import { Activity, Clock, CheckCircle2, TrendingUp } from 'lucide-react';
import { Skeleton } from '@/components/ui/skeleton';

export const AnalyticsKpiCards = ({ projectId }: { projectId: string }) => {
    const { data: velocityData, isLoading: loadingVelocity } = useVelocityQuery(projectId);
    const { data: cycleTimeData, isLoading: loadingCycle } = useCycleTimeQuery(projectId);

    if (loadingVelocity || loadingCycle) {
        return (
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 mb-6">
                {[1, 2, 3, 4].map(i => <Skeleton key={i} className="h-28 rounded-xl" />)}
            </div>
        );
    }

    // Calculations
    const avgVelocity = velocityData && velocityData.length > 0
        ? Math.round(velocityData.reduce((acc, curr) => acc + curr.completedTasks, 0) / velocityData.length)
        : 0;

    const avgCycleTime = cycleTimeData && cycleTimeData.length > 0
        ? (cycleTimeData.reduce((acc, curr) => acc + curr.cycleTimeDays, 0) / cycleTimeData.length).toFixed(1)
        : '0.0';

    const avgLeadTime = cycleTimeData && cycleTimeData.length > 0
        ? (cycleTimeData.reduce((acc, curr) => acc + curr.leadTimeDays, 0) / cycleTimeData.length).toFixed(1)
        : '0.0';

    const totalCompleted = velocityData?.reduce((acc, curr) => acc + curr.completedTasks, 0) || 0;

    const cards = [
        { title: 'Avg. Sprint Velocity', value: avgVelocity, subtitle: 'Tasks per sprint', icon: Activity, color: 'text-indigo-500' },
        { title: 'Avg. Cycle Time', value: `${avgCycleTime}d`, subtitle: 'Work started to completion', icon: Clock, color: 'text-amber-500' },
        { title: 'Avg. Lead Time', value: `${avgLeadTime}d`, subtitle: 'Creation to completion', icon: TrendingUp, color: 'text-blue-500' },
        { title: 'Total Tasks Delivered', value: totalCompleted, subtitle: 'Across all tracked sprints', icon: CheckCircle2, color: 'text-emerald-500' },
    ];

    return (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 mb-8">
            {cards.map((card, idx) => {
                const Icon = card.icon;
                return (
                    <div key={idx} className="bg-white dark:bg-zinc-900 border border-zinc-200 dark:border-zinc-800 rounded-xl p-5 shadow-sm flex items-center gap-4">
                        <div className={`p-3 rounded-lg bg-zinc-50 dark:bg-zinc-800/50 ${card.color}`}>
                            <Icon size={24} />
                        </div>
                        <div>
                            <p className="text-sm font-medium text-zinc-500 dark:text-zinc-400">{card.title}</p>
                            <h4 className="text-2xl font-bold dark:text-zinc-50 mt-0.5">{card.value}</h4>
                            <p className="text-xs text-zinc-400 mt-1">{card.subtitle}</p>
                        </div>
                    </div>
                );
            })}
        </div>
    );
};