import { useMutation, useQueryClient } from '@tanstack/react-query';
import { login } from '../api/mutations';
import { queryKeys } from './queryKeys';

// TODO
export function useLogin() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: login,
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.user,
      });
    },
  });
}
