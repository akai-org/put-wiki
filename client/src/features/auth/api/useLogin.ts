import { useMutation } from '@tanstack/react-query';
import { login } from '../api/mutations';

// No cache invalidation needed - login ends with a full-page navigation
export function useLogin() {
  return useMutation({
    mutationFn: (returnUrl?: string) => login(returnUrl),
  });
}
