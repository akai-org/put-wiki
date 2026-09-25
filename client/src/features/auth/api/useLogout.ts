import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useRouter } from '@tanstack/react-router';
import { logout } from '../api/mutations';
import { queryKeys } from '../api/queryKeys';

export function useLogout() {
  const queryClient = useQueryClient();
  const router = useRouter();

  return useMutation({
    mutationFn: logout,
    onSuccess: async () => {
      queryClient.setQueryData(queryKeys.user, null);

      queryClient.removeQueries({
        predicate: (query) => query.queryKey[0] !== 'auth',
      });

      // re-runs beforeLoad guards, so user is redirected away from protected routes
      await router.invalidate();
    },
  });
}
