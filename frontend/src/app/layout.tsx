import './globals.css';
import { AuthProvider } from '@/providers/AuthProvider';
import { Geist } from "next/font/google";
import { cn } from "@/lib/utils";

const geist = Geist({subsets:['latin'],variable:'--font-sans'});


// Assume QueryClientProvider is also configured here

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en" className={cn("dark", "font-sans", geist.variable)}>
      <body className="antialiased text-zinc-900 bg-white dark:bg-zinc-950 dark:text-zinc-50">
         {/* QueryClientProvider should wrap AuthProvider in real app */}
         <AuthProvider>
           {children}
         </AuthProvider>
      </body>
    </html>
  );
}