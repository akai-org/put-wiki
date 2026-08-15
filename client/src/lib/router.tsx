import { HomePage, NotFoundPage, CoursePage, DegreeCoursePage } from '@/pages';
import { createRootRoute, createRoute, createRouter, Outlet } from '@tanstack/react-router';
import Header from '@/components/Header/Header';
// import Footer from '@/components/Footer/Footer';

const rootRoute = createRootRoute({
  component: () => (
    <div className="flex flex-col min-h-screen bg-[#121318]">
      <Header />
      <main className="flex-grow">
        <Outlet />
      </main>
      {/* <Footer /> */}
    </div>
  ),
});

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
