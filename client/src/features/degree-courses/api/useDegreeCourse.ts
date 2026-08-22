import { useQuery } from '@tanstack/react-query';
import { degreeCourseQueries } from './getDegreeCourse';

export function useDegreeCourse(slug: string) {
  return useQuery(degreeCourseQueries.detail(slug));
}
