import { queryOptions } from '@tanstack/react-query';
import { api } from '@/lib/api';
import { userPublicProfileKeys } from './queryKeys';
import {
  type UserPublic,
  UserPublicSchema,
} from '@/features/user-public-profile/userPublicProfileSchema';

async function getUserPublicProfile(nickname: string): Promise<UserPublic> {
  const response = await api.get(`/profile/${nickname}`);
  console.log(response.data);
  return UserPublicSchema.parse(response.data);
}

const userPublicProfileQueries = {
  bySlug: (nickname: string) =>
    queryOptions({
      queryKey: userPublicProfileKeys.bySlug(nickname),
      queryFn: () => getUserPublicProfile(nickname),
      staleTime: 1000 * 60 * 5, // 5 minutes
    }),
};

export { userPublicProfileQueries };
