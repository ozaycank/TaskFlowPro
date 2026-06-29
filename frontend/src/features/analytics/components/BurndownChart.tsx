'use client';

import { useBurndownQuery } from '../hooks/useAnalyticsHooks';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import { Skeleton } from '@/components/ui/skeleton';
import { format } from 'date-fns';

export const BurndownChart = ({ sprintId }: { sprintId: string | undefined }) => {
    const { data, isLoading } = useBurndownQuery(sprintId);

    if (!sprintId) return <div className="h-[300px] flex items-center justify-center text-zinc-500">Select an active sprint to view burndown.</div>;
    if (isLoading) return <Skeleton className="w-full h-[300px] rounded-xl" />;
    if (!data || data.length === 0) return <div className="h-[300px] flex items-center justify-center text-zinc-500">No burndown data available.</div>;

    const formattedData = data.map(d => ({
        ...d,
        dateFormatted: format(new Date(d.date), 'MMM dd')
    }));

    return (
        <div className="h-[300px] w-full">
            <ResponsiveContainer width="100%" height="100%">
                <LineChart data={formattedData} margin={{ top: 10, right: 30, left: 0, bottom: 0 }}>
                    <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="#3f3f46" opacity={0.2} />
                    <XAxis dataKey="dateFormatted" axisLine={false} tickLine={false} tick={{ fontSize: 12 }} />
                    <YAxis axisLine={false} tickLine={false} tick={{ fontSize: 12 }} />
                    <Tooltip contentStyle={{ borderRadius: '8px', border: 'none', boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1)' }} />
                    <Legend iconType="plainline" wrapperStyle={{ fontSize: '12px' }} />
                    <Line type="monotone" dataKey="idealTasks" name="Ideal Trend" stroke="#94a3b8" strokeWidth={2} strokeDasharray="5 5" dot={false} />
                    <Line type="monotone" dataKey="remainingTasks" name="Actual Remaining" stroke="#ef4444" strokeWidth={3} dot={{ r: 4 }} activeDot={{ r: 6 }} />
                </LineChart>
            </ResponsiveContainer>
        </div>
    );
};