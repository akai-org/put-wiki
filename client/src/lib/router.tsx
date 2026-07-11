import { HomePage, NotFoundPage, CoursePage, DegreeCoursePage } from '@/pages';
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

const degreeCoursePageRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/degree-course/$slug',
  component: () => <DegreeCoursePage />,
});
const routeTree = rootRoute.addChildren([
  indexRoute,
  coursePageRoute,
  degreeCoursePageRoute,
  notFoundRoute,
]);
const router = createRouter({ routeTree });

export default router;
