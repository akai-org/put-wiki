import { useQuery } from '@tanstack/react-query';
import { courseQueries } from '@/features/courses/api/courseQueries';

export function useCourseQuery(slug: string) {
  return useQuery(courseQueries.detail(slug));
}
