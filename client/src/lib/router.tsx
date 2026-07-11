import { HomePage, NotFoundPage, CoursePage, ToasterPage } from '@/pages';
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
  path: '/course/$slug',
  component: () => <CoursePage />,
});

const toasterPageRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/toasters',
  component: () => <ToasterPage />,
});

const routeTree = rootRoute.addChildren([
  indexRoute,
  coursePageRoute,
  toasterPageRoute,
  notFoundRoute,
]);
const router = createRouter({ routeTree });

export default router;
