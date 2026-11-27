import { useState, type ReactNode } from 'react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ThemeProvider } from '@/contexts/ThemeProvider';
import type { Theme } from '@/contexts/ThemeContext';
import { RouterDecorator } from './RouterDecorator';

export interface AppProvidersOptions {
  initialPath?: string;
  theme?: Theme;
}

/** Theme, React Query and router providers for tests and stories. */
export function AppProviders({
  children,
  initialPath,
  theme,
}: AppProvidersOptions & { children: ReactNode }) {
  const [queryClient] = useState(
    () => new QueryClient({ defaultOptions: { queries: { retry: false } } })
  );

  return (
    <ThemeProvider initialTheme={theme}>
      <QueryClientProvider client={queryClient}>
        <RouterDecorator initialPath={initialPath}>{children}</RouterDecorator>
      </QueryClientProvider>
    </ThemeProvider>
  );
}
