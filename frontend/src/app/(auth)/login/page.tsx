import { LoginForm } from '@/features/authentication/components/LoginForm';
import { Metadata } from 'next';

export const metadata: Metadata = {
  title: 'Login - Velyo',
  description: 'Sign in to your Velyo workspace.',
};

export default function LoginPage() {
  return <LoginForm />;
}