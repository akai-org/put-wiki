import { HomePage, NotFoundPage, PrivateProfile, PublicProfile } from '@/pages';
import { createRootRoute, createRoute, createRouter } from '@tanstack/react-router';

const rootRoute = createRootRoute();

const indexRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/',
  component: () => <HomePage />,
});

const privateProfileRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/profile',
  component: () => <PrivateProfile />,
});

const publicProfileRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/user/$userid',
  component: () => <PublicProfile />,
});

const notFoundRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '$',
  component: () => <NotFoundPage />,
});

const routeTree = rootRoute.addChildren([
  indexRoute,
  privateProfileRoute,
  publicProfileRoute,
  notFoundRoute,
]);

const router = createRouter({ routeTree });

export default router;
