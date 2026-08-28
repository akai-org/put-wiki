import {
  createRootRouteWithContext,
  type ErrorComponentProps,
  Outlet,
} from '@tanstack/react-router';
import GlobalNotFoundPage from './-not-found';
import GlobalErrorPage from './-error';
import { MainLayout } from '@/components/layout/MainLayout';
import type { QueryClient } from '@tanstack/react-query';

interface RouterContext {
  queryClient: QueryClient;
}

export const Route = createRootRouteWithContext<RouterContext>()({
  component: Root,
  notFoundComponent: GlobalNotFoundPage,
  errorComponent: (props: ErrorComponentProps) => (
    <MainLayout>
      <GlobalErrorPage {...props} />
    </MainLayout>
  ),
});

function Root() {
  return (
    <MainLayout>
      <Outlet />
    </MainLayout>
  );
}
