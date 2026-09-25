import { useQuery } from '@tanstack/react-query';
import { lecturerQueries } from './lecturerQueries';

export function useLecturerQuery(slug: string) {
  return useQuery(lecturerQueries.bySlug(slug));
}
