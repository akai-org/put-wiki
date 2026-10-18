import axios from 'axios';
import {
  createFileRoute,
  notFound,
  type FileRoutesByPath,
  type RouteComponent,
} from '@tanstack/react-router';
import { lecturerQueries } from '@/features/lecturers';
import { degreeCourseQueries } from '@/features/degree-courses';
import { courseQueries } from '@/features/courses';

type Queries = typeof lecturerQueries | typeof degreeCourseQueries | typeof courseQueries;

// Creates a route using tanstack routes
// Usage: createRoute(URL to the page, component to render, component to render while loading, queries to prefetch)
export default function createRoute(
  url: keyof FileRoutesByPath,
  component: RouteComponent,
  pendingComponent: RouteComponent,
  queries: Queries
) {
  return createFileRoute(url)({
    component: component,
    pendingComponent: pendingComponent,
    loader: async ({ context: { queryClient }, params: { slug } }) => {
      try {
        await queryClient.ensureQueryData(
          queries.bySlug(slug) as Parameters<typeof queryClient.ensureQueryData>[0]
        );
      } catch (error) {
        if (axios.isAxiosError(error) && error.response?.status === 404) {
          throw notFound();
        }
        throw error;
      }
    },
  });
}
