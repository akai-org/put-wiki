import { HomePage, NotFoundPage, LoginPage, ProfileSetupPage } from '@/pages';
import { createRootRoute, createRoute, createRouter } from '@tanstack/react-router';

const rootRoute = createRootRoute();

const indexRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/',
  component: () => <HomePage />,
});

const notFoundRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '$',
  component: () => <NotFoundPage />,
});

const profileSetupRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/profile-setup',
  component: () => <ProfileSetupPage />,
});

const routeTree = rootRoute.addChildren([indexRoute, notFoundRoute, loginRoute, profileSetupRoute]);
const router = createRouter({ routeTree });

export default router;
