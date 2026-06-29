'use client';

import { useVelocityQuery } from '../hooks/useAnalyticsHooks';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import { Skeleton } from '@/components/ui/skeleton';

export const VelocityChart = ({ projectId }: { projectId: string }) => {
    const { data, isLoading } = useVelocityQuery(projectId);

    if (isLoading) return <Skeleton className="w-full h-[300px] rounded-xl" />;
    if (!data || data.length === 0) return <div className="h-[300px] flex items-center justify-center text-zinc-500">No velocity data available.</div>;

    return (
        <div className="h-[300px] w-full">
            <ResponsiveContainer width="100%" height="100%">
                <BarChart data={data} margin={{ top: 10, right: 30, left: 0, bottom: 0 }}>
                    <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="#3f3f46" opacity={0.2} />
                    <XAxis dataKey="sprintName" axisLine={false} tickLine={false} tick={{ fontSize: 12 }} />
                    <YAxis axisLine={false} tickLine={false} tick={{ fontSize: 12 }} />
                    <Tooltip cursor={{ fill: 'transparent' }} contentStyle={{ borderRadius: '8px', border: 'none', boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1)' }} />
                    <Legend iconType="circle" wrapperStyle={{ fontSize: '12px' }} />
                    <Bar dataKey="totalTasks" name="Total Committed" fill="#94a3b8" radius={[4, 4, 0, 0]} />
                    <Bar dataKey="completedTasks" name="Completed" fill="#6366f1" radius={[4, 4, 0, 0]} />
                </BarChart>
            </ResponsiveContainer>
        </div>
    );
};