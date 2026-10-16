import { useSuspenseQuery } from '@tanstack/react-query';
import { degreeCourseQueries } from './degreeCourseQueries';

export function useDegreeCourseQuery(slug: string) {
  return useSuspenseQuery(degreeCourseQueries.bySlug(slug));
}
