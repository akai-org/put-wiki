import { queryOptions } from '@tanstack/react-query';
import { api } from '@/lib/api';
import type { User } from '@/features/auth/userSchema';
import { queryKeys } from '@/features/auth/api/queryKeys';

async function getUser(): Promise<User> {
  const response = await api.get<User>(`/users/me`);
  return response.data;
}

const queries = {
  user: () =>
    queryOptions({
      queryKey: queryKeys.user,
      queryFn: getUser,
      staleTime: 1000 * 60 * 5, // 5 minutes
    }),
};

export { queries };
