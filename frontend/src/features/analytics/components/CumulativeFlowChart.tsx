'use client';

import { useMemo } from 'react';
import { useCFDQuery } from '../hooks/useAnalyticsHooks';
import { AreaChart, Area, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import { Skeleton } from '@/components/ui/skeleton';
import { format } from 'date-fns';

const CHART_COLORS = ['#6366f1', '#10b981', '#f59e0b', '#8b5cf6', '#ec4899', '#06b6d4'];

export const CumulativeFlowChart = ({ projectId }: { projectId: string }) => {
    const { data, isLoading } = useCFDQuery(projectId);

    // Recharts AreaChart requires pivoted data. We transform [{date, state, count}] to [{date, State1: count, State2: count}]
    const { chartData, states } = useMemo(() => {
        if (!data || data.length === 0) return { chartData: [], states: [] };

        const grouped: Record<string, any> = {};
        const stateSet = new Set<string>();

        data.forEach(item => {
            const dateStr = format(new Date(item.date), 'MMM dd');
            if (!grouped[dateStr]) grouped[dateStr] = { dateFormatted: dateStr };
            grouped[dateStr][item.stateName] = item.taskCount;
            stateSet.add(item.stateName);
        });

        // Ensure chronological order if needed (assuming backend returns chronologically)
        return { 
            chartData: Object.values(grouped), 
            states: Array.from(stateSet) 
        };
    }, [data]);

    if (isLoading) return <Skeleton className="w-full h-[300px] rounded-xl" />;
    if (chartData.length === 0) return <div className="h-[300px] flex items-center justify-center text-zinc-500">No cumulative flow data available.</div>;

    return (
        <div className="h-[300px] w-full">
            <ResponsiveContainer width="100%" height="100%">
                <AreaChart data={chartData} margin={{ top: 10, right: 30, left: 0, bottom: 0 }}>
                    <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="#3f3f46" opacity={0.2} />
                    <XAxis dataKey="dateFormatted" axisLine={false} tickLine={false} tick={{ fontSize: 12 }} />
                    <YAxis axisLine={false} tickLine={false} tick={{ fontSize: 12 }} />
                    <Tooltip contentStyle={{ borderRadius: '8px', border: 'none', boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1)' }} />
                    <Legend iconType="circle" wrapperStyle={{ fontSize: '12px' }} />
                    
                    {states.map((stateName, index) => (
                        <Area 
                            key={stateName}
                            type="monotone" 
                            dataKey={stateName} 
                            stackId="1" 
                            stroke={CHART_COLORS[index % CHART_COLORS.length]} 
                            fill={CHART_COLORS[index % CHART_COLORS.length]} 
                            fillOpacity={0.6}
                        />
                    ))}
                </AreaChart>
            </ResponsiveContainer>
        </div>
    );
};