import { HomePage, NotFoundPage, CoursePage } from '@/pages';
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

const coursePageRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/course/$courseId',
  component: () => <CoursePage />,
});

const routeTree = rootRoute.addChildren([indexRoute, coursePageRoute, notFoundRoute]);
const router = createRouter({ routeTree });

export default router;
