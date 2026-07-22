'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import Link from 'next/link';
import Image from 'next/image'; // YENİ EKLENDİ
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { registerSchema, RegisterFormData } from '../schemas/auth.schema';
import { useRegisterMutation } from '../hooks/useRegisterMutation';

export const RegisterForm = () => {
    const { mutate, isPending } = useRegisterMutation();
    const [apiError, setApiError] = useState<string | null>(null);

    const { register, handleSubmit, formState: { errors } } = useForm<RegisterFormData>({
        resolver: zodResolver(registerSchema)
    });

    const onSubmit = (data: RegisterFormData) => {
        setApiError(null);
        mutate(data, {
            onError: (error: any) => {
                const message = error.response?.data?.message || error.response?.data?.title || 'Registration failed. Please try again.';
                setApiError(message);
            }
        });
    };

    return (
        <div className="w-full max-w-md space-y-6 bg-white dark:bg-zinc-950 p-8 rounded-2xl shadow-xl border border-zinc-200 dark:border-zinc-800">
            <div className="space-y-2 text-center flex flex-col items-center">
                <Image 
                    src="/logo.png" 
                    alt="Velyo Logo" 
                    width={48} 
                    height={48} 
                    className="mb-4 rounded-xl"
                />
                <h1 className="text-3xl font-bold tracking-tight text-zinc-900 dark:text-white">Create an account</h1>
                <p className="text-sm text-zinc-500 dark:text-zinc-400">
                    Enter your details below to create your workspace
                </p>
            </div>

            {apiError && (
                <div className="p-3 text-sm text-red-600 bg-red-50 dark:bg-red-900/20 dark:text-red-400 rounded-md border border-red-200 dark:border-red-900/30">
                    {apiError}
                </div>
            )}

            <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                <div className="grid grid-cols-2 gap-4">
                    <div className="space-y-2">
                        <Label htmlFor="firstName">First name</Label>
                        <Input id="firstName" placeholder="John" {...register('firstName')} />
                        {errors.firstName && <span className="text-xs text-red-500">{errors.firstName.message}</span>}
                    </div>
                    <div className="space-y-2">
                        <Label htmlFor="lastName">Last name</Label>
                        <Input id="lastName" placeholder="Doe" {...register('lastName')} />
                        {errors.lastName && <span className="text-xs text-red-500">{errors.lastName.message}</span>}
                    </div>
                </div>

                <div className="space-y-2">
                    <Label htmlFor="email">Email</Label>
                    <Input id="email" type="email" placeholder="john@company.com" {...register('email')} />
                    {errors.email && <span className="text-xs text-red-500">{errors.email.message}</span>}
                </div>

                <div className="space-y-2">
                    <Label htmlFor="password">Password</Label>
                    <Input id="password" type="password" placeholder="••••••••" {...register('password')} />
                    {errors.password && <span className="text-xs text-red-500">{errors.password.message}</span>}
                </div>

                <Button type="submit" className="w-full" disabled={isPending}>
                    {isPending ? 'Creating account...' : 'Create account'}
                </Button>
            </form>

            <div className="text-center text-sm text-zinc-500 dark:text-zinc-400">
                Already have an account?{' '}
                <Link href="/login" className="font-semibold text-indigo-600 hover:text-indigo-500 dark:text-indigo-400">
                    Sign in
                </Link>
            </div>
        </div>
    );
};