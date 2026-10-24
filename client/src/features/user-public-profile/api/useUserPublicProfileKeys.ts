import { useQuery } from '@tanstack/react-query';
import { userPublicProfileQueries } from './userPublicProfileQueries';

export function useUserPublicProfileQuery(nicknameSlug: string) {
  return useQuery(userPublicProfileQueries.bySlug(nicknameSlug));
}
