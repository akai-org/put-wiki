import { RouterProvider } from '@tanstack/react-router';
import router from '@/lib/router';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { QueryClientProvider } from '@tanstack/react-query';
import { ThemeProvider } from '@/contexts/ThemeProvider';
import { ToastsProvider } from '@/components/ui/ToastsProvider';
import { TanStackRouterDevtools } from '@tanstack/react-router-devtools';
import { queryClient } from '@/lib/queryClient';

export function App() {
  return (
    <ThemeProvider>
      <QueryClientProvider client={queryClient}>
        <RouterProvider router={router} />
        <ToastsProvider position="top-center" />
        <TanStackRouterDevtools router={router} />
        <ReactQueryDevtools />
      </QueryClientProvider>
    </ThemeProvider>
  );
}
