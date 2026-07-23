import './globals.css';
import { AuthProvider } from '@/providers/AuthProvider';
import { QueryProvider } from '@/providers/QueryProvider';
import { ThemeProvider } from '@/providers/ThemeProvider';
import { Geist } from 'next/font/google';

const geist = Geist({ subsets: ['latin'] });

export const metadata = {
  title: 'Velyo - Enterprise Workspace',
  description: 'Manage your projects, sprints and workflows efficiently.',
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en" suppressHydrationWarning>
      <body className={`${geist.className} antialiased text-zinc-900 bg-white dark:bg-zinc-950 dark:text-zinc-50`}>
         <ThemeProvider attribute="class" defaultTheme="system" enableSystem disableTransitionOnChange>
           <QueryProvider>
             <AuthProvider>
               {children}
             </AuthProvider>
           </QueryProvider>
         </ThemeProvider>
      </body>
    </html>
  );
}