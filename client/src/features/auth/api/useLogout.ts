import { useMutation, useQueryClient } from '@tanstack/react-query';
import { logout } from '../api/mutations';
import { queryKeys } from '../api/queryKeys';

// TODO
export function useLogout() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: logout,
    onSuccess: () => {
      queryClient.setQueryData(queryKeys.user, null);

      queryClient.removeQueries({
        predicate: (query) => query.queryKey[0] !== 'auth',
      });
    },
  });
}
