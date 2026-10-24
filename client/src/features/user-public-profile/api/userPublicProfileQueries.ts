import { queryOptions } from '@tanstack/react-query';
import { api } from '@/lib/api';
import { userPublicProfileKeys } from './queryKeys';
import { type UserPublic, UserPublicSchema } from '../schemas/userPublicProfileSchema';

async function getUserPublicProfile(nicknameSlug: string): Promise<UserPublic> {
  const response = await api.get(`/profiles/${nicknameSlug}`);
  return UserPublicSchema.parse(response.data);
}

const userPublicProfileQueries = {
  bySlug: (nicknameSlug: string) =>
    queryOptions({
      queryKey: userPublicProfileKeys.bySlug(nicknameSlug),
      queryFn: () => getUserPublicProfile(nicknameSlug),
      staleTime: 1000 * 60 * 5, // 5 minutes
    }),
};

export { userPublicProfileQueries };
