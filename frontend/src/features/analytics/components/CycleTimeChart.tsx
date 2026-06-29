'use client';

import { useCycleTimeQuery } from '../hooks/useAnalyticsHooks';
import { ScatterChart, Scatter, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';
import { Skeleton } from '@/components/ui/skeleton';

export const CycleTimeChart = ({ projectId }: { projectId: string }) => {
    const { data, isLoading } = useCycleTimeQuery(projectId);

    if (isLoading) return <Skeleton className="w-full h-[300px] rounded-xl" />;
    if (!data || data.length === 0) return <div className="h-[300px] flex items-center justify-center text-zinc-500">No cycle time data available.</div>;

    return (
        <div className="h-[300px] w-full">
            <ResponsiveContainer width="100%" height="100%">
                <ScatterChart margin={{ top: 20, right: 20, bottom: 20, left: 0 }}>
                    <CartesianGrid strokeDasharray="3 3" stroke="#3f3f46" opacity={0.2} />
                    <XAxis type="category" dataKey="taskTitle" name="Task" hide />
                    <YAxis type="number" dataKey="cycleTimeDays" name="Days" unit="d" axisLine={false} tickLine={false} tick={{ fontSize: 12 }} />
                    <Tooltip cursor={{ strokeDasharray: '3 3' }} contentStyle={{ borderRadius: '8px', border: 'none', boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1)' }} />
                    <Scatter name="Tasks" data={data} fill="#10b981" />
                </ScatterChart>
            </ResponsiveContainer>
        </div>
    );
};