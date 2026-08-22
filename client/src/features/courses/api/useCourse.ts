import { useQuery } from '@tanstack/react-query';
import { courseQueries } from '@/features/courses/api/getCourse';

export function useCourse(slug: string) {
  return useQuery(courseQueries.detail(slug));
}
