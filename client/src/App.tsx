import { RouterProvider } from '@tanstack/react-router';
import router from '@/lib/router';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ThemeProvider } from '@/contexts/ThemeProvider';
import { AuthProvider } from '@/contexts/AuthProvider';
import { UserProvider } from '@/contexts/UserProvider';
import { Toaster } from '@/components/ui/sonner.tsx';

const queryClient = new QueryClient();

export function App() {
  return (
    <ThemeProvider>
      <AuthProvider>
        <UserProvider>
          <QueryClientProvider client={queryClient}>
            <RouterProvider router={router} />
            <ReactQueryDevtools initialIsOpen={false} />
            <Toaster />
          </QueryClientProvider>
        </UserProvider>
      </AuthProvider>
    </ThemeProvider>
  );
}
