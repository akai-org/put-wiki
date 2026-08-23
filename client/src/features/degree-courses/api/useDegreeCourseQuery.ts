import { useQuery } from '@tanstack/react-query';
import { degreeCourseQueries } from './degreeCourseQueries';

export function useDegreeCourseQuery(slug: string) {
  return useQuery(degreeCourseQueries.detail(slug));
}
