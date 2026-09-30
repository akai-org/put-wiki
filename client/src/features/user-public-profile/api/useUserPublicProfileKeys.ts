import { useQuery } from '@tanstack/react-query';
import { userPublicProfileQueries } from './userPublicProfileQueries';

export function useUserPublicProfileQuery(slug: string) {
  return useQuery(userPublicProfileQueries.bySlug(slug));
}
