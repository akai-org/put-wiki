import { queryOptions } from '@tanstack/react-query';
import { api } from '@/lib/api';
import { userPublicProfileKeys } from './queryKeys';
import {
  type UserPublic,
  UserPublicSchema,
} from '@/features/user-public-profile/userPublicProfileSchema';

async function getUserPublicProfile(slug: string): Promise<UserPublic> {
  const response = await api.get(`/mocks/${slug}.json`);
  return UserPublicSchema.parse(response.data);
}

const userPublicProfileQueries = {
  bySlug: (slug: string) =>
    queryOptions({
      queryKey: userPublicProfileKeys.bySlug(slug),
      queryFn: () => getUserPublicProfile(slug),
      staleTime: 1000 * 60 * 5, // 5 minutes
    }),
};

export { userPublicProfileQueries };
