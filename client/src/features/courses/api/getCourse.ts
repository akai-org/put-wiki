import { queryOptions } from '@tanstack/react-query';
import { api } from '@/lib/api';
import { courseKeys } from './queryKeys';
import { type Course, CourseSchema } from '@/schemas/course';

async function getCourse(slug: string): Promise<Course> {
  const response = await api.get(`/mocks/${slug}.json`);
  return CourseSchema.parse(response.data);
}

const courseQueries = {
  detail: (slug: string) =>
    queryOptions({
      queryKey: courseKeys.detail(slug),
      queryFn: () => getCourse(slug),
      staleTime: 1000 * 60 * 5, // 5 minutes
    }),
};

export { courseQueries };
