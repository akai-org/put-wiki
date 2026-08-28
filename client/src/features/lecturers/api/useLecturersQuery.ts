import { useQuery } from '@tanstack/react-query';
import { LecturerQueries } from './LecturerQueries';

export function useLecturerQuery(slug: string) {
  return useQuery(LecturerQueries.bySlug(slug));
}
