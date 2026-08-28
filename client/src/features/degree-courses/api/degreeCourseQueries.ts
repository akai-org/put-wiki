import { queryOptions } from '@tanstack/react-query';
import { api } from '@/lib/api';
import {
  DegreeCourseSchema,
  type DegreeCourse,
} from '@/features/degree-courses/degreeCourseSchema';
import { degreeCourseKeys } from './queryKeys';

async function getDegreeCourse(slug: string): Promise<DegreeCourse> {
  const response = await api.get(`/mocks/${slug}.json`);
  return DegreeCourseSchema.parse(response.data);
}

const degreeCourseQueries = {
  bySlug: (slug: string) =>
    queryOptions({
      queryKey: degreeCourseKeys.bySlug(slug),
      queryFn: () => getDegreeCourse(slug),
      staleTime: 1000 * 60 * 5, // 5 minutes
    }),
};

export { degreeCourseQueries };
