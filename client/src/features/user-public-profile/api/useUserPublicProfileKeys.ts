import { useQuery } from '@tanstack/react-query';
import { userPublicProfileQueries } from '@/features/user-public-profile/api/userPublicProfileQueries';

export function useUserPublicProfileQuery(slug: string) {
  return useQuery(userPublicProfileQueries.bySlug(slug));
}
