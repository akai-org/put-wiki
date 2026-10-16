import { useSuspenseQuery } from '@tanstack/react-query';
import { courseQueries } from '@/features/courses/api/courseQueries';

export function useCourseQuery(slug: string) {
  return useSuspenseQuery(courseQueries.bySlug(slug));
}
