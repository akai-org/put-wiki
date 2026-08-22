import { queryOptions } from '@tanstack/react-query';
import { api } from '@/lib/api';
import { DegreeCourseSchema, type DegreeCourse } from '@/schemas/degreeCourse';
import { degreeCourseKeys } from './queryKeys';

async function getDegreeCourse(slug: string): Promise<DegreeCourse> {
  const response = await api.get(`/mocks/${slug}.json`);
  return DegreeCourseSchema.parse(response.data);
}

const degreeCourseQueries = {
  detail: (slug: string) =>
    queryOptions({
      queryKey: degreeCourseKeys.detail(slug),
      queryFn: () => getDegreeCourse(slug),
      staleTime: 1000 * 60 * 5, // 5 minutes
    }),
};

export { degreeCourseQueries };
