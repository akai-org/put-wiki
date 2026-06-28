import { HomePage, NotFoundPage, CoursePage, LecturerPage } from '@/pages';
import { createRootRoute, createRoute, createRouter } from '@tanstack/react-router';

const rootRoute = createRootRoute();

const indexRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/',
  component: () => <HomePage />,
});

const coursePageRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/course/$slug',
  component: () => <CoursePage />,
});

const lecturerPageRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/lecturer',
  component: () => <LecturerPage />,
});

const notFoundRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '$',
  component: () => <NotFoundPage />,
});

const routeTree = rootRoute.addChildren([
  indexRoute,
  coursePageRoute,
  lecturerPageRoute,
  notFoundRoute,
]);
const router = createRouter({ routeTree });

export default router;
