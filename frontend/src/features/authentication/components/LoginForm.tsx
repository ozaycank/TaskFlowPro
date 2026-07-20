'use client';

import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { loginSchema, LoginFormData } from '../schemas/auth.schema';
import { useLoginMutation } from '../hooks/useLoginMutation';
import { useRouter } from 'next/navigation';
import { useState } from 'react';
import { AxiosError } from 'axios';
import Link from 'next/link';

export const LoginForm = () => {
  const router = useRouter();
  const loginMutation = useLoginMutation();
  const [globalError, setGlobalError] = useState<string | null>(null);

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<LoginFormData>({
    resolver: zodResolver(loginSchema),
    defaultValues: { email: '', password: '' },
  });

  const onSubmit = async (data: LoginFormData) => {
    setGlobalError(null);
    loginMutation.mutate(data, {
      onSuccess: () => {
        router.push('/'); // Redirect to dashboard
      },
      onError: (error: Error) => {
        const axiosError = error as AxiosError<any>;
        const problem = axiosError.response?.data;
        setGlobalError(problem?.detail || 'Invalid email or password.');
      },
    });
  };

  return (
    <div className="w-full max-w-md p-8 space-y-6 bg-white dark:bg-zinc-900 rounded-xl shadow-lg border border-zinc-200 dark:border-zinc-800">
      <div className="text-center">
        <h1 className="text-2xl font-bold tracking-tight text-zinc-900 dark:text-white">Welcome back</h1>
        <p className="text-sm text-zinc-500 dark:text-zinc-400 mt-2">Enter your credentials to access your workspace.</p>
      </div>

      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        {globalError && (
          <div className="p-3 text-sm text-red-600 bg-red-50 dark:bg-red-900/20 dark:text-red-400 rounded-md">
            {globalError}
          </div>
        )}

        <div className="space-y-2">
          <label className="text-sm font-medium text-zinc-900 dark:text-zinc-300">Email Address</label>
          <input
            {...register('email')}
            type="email"
            className="w-full px-3 py-2 border rounded-md dark:bg-zinc-950 dark:border-zinc-800 focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="name@company.com"
          />
          {errors.email && <p className="text-sm text-red-500">{errors.email.message}</p>}
        </div>

        <div className="space-y-2">
          <label className="text-sm font-medium text-zinc-900 dark:text-zinc-300">Password</label>
          <input
            {...register('password')}
            type="password"
            className="w-full px-3 py-2 border rounded-md dark:bg-zinc-950 dark:border-zinc-800 focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="••••••••"
          />
          {errors.password && <p className="text-sm text-red-500">{errors.password.message}</p>}
        </div>

        <button
          type="submit"
          disabled={isSubmitting || loginMutation.isPending}
          className="w-full py-2.5 text-sm font-semibold text-white bg-blue-600 rounded-md hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
        >
          {loginMutation.isPending ? 'Signing in...' : 'Sign In'}
        </button>
      </form>

      {/* YENİ: Register Sayfasına Yönlendirme Linki */}
      <div className="text-center text-sm text-zinc-500 dark:text-zinc-400 pt-2">
        Don't have an account?{' '}
        <Link href="/register" className="font-semibold text-blue-600 hover:text-blue-500 dark:text-blue-400">
            Sign up
        </Link>
      </div>
    </div>
  );
};