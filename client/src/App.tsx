import { RouterProvider } from '@tanstack/react-router';
import router from '@/lib/router';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ThemeProvider } from '@/contexts/ThemeProvider';
import { AuthProvider } from '@/contexts/AuthProvider';
import { UserProvider } from '@/contexts/UserProvider';
import { ToastsProvider } from '@/components/ui/ToastsProvider';
import { TanStackRouterDevtools } from '@tanstack/router-devtools';

const queryClient = new QueryClient();

export function App() {
  return (
    <ThemeProvider>
      <AuthProvider>
        <UserProvider>
          <QueryClientProvider client={queryClient}>
            <RouterProvider router={router} />
            <ToastsProvider position="top-center" />
            <TanStackRouterDevtools router={router} />
            <ReactQueryDevtools />
          </QueryClientProvider>
        </UserProvider>
      </AuthProvider>
    </ThemeProvider>
  );
}
