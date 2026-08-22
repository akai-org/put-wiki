import { createRootRoute, type ErrorComponentProps, Outlet } from '@tanstack/react-router';
import GlobalNotFoundPage from './-not-found';
import GlobalErrorPage from './-error';
import { MainLayout } from '@/components/layout/MainLayout';

export const Route = createRootRoute({
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
