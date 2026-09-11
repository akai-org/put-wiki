import { useQuery } from '@tanstack/react-query';
import { useLogin } from '../api/useLogin';
import { useLogout } from '../api/useLogout';
import { queries } from '../api/queries';

export function useUserSession() {
  const { data: user, isLoading, isError } = useQuery(queries.user());

  const loginMutation = useLogin();
  const logoutMutation = useLogout();

  return {
    user: user ?? null,
    isLoggedIn: !!user,
    isLoading,
    isError,

    login: (returnUrl?: string) => {
      loginMutation.mutate(returnUrl);
    },
    logout: () => {
      logoutMutation.mutate();
    },

    isLoggingIn: loginMutation.isPending,
    isLoggingOut: logoutMutation.isPending,
  };
}
