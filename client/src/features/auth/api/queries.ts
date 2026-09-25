import { queryOptions } from '@tanstack/react-query';
import { isAxiosError } from 'axios';
import { api } from '@/lib/api';
import { type User, UserSchema } from '../userSchema';
import { queryKeys } from './queryKeys';

// JWT is kept in httpOnly `auth_token` cookie, so the only way to know whether user is logged in is to ask the backend.
// 401 means "not logged in" (no cookie, invalid or expired token) and is not treated as an error.
async function getUser(): Promise<User | null> {
  try {
    const response = await api.get('/api/user/profile');
    return UserSchema.parse(response.data);
  } catch (error) {
    if (isAxiosError(error) && error.response?.status === 401) {
      return null;
    }
    throw error;
  }
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
