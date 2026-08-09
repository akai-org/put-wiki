import { HomePage, NotFoundPage, CoursePage, DegreeCoursePage, LecturerPage } from '@/pages';
import { createRootRoute, createRoute, createRouter, Outlet } from '@tanstack/react-router';
import Header from '@/components/Header/Header';
// import Footer from '@/components/Footer/Footer';

const rootRoute = createRootRoute({
  component: () => (
    <div className="flex flex-col min-h-screen">
      <Header />
      <main className="flex">
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

const lecturerPageRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/lecturers/$slug',
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
  degreeCoursePageRoute,
 
  lecturerPageRoute,
  notFoundRoute,
,
]);
const router = createRouter({ routeTree });

export default router;
