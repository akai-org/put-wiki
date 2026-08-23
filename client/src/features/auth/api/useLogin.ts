import { useMutation } from '@tanstack/react-query';
import { login } from '@/features/auth/api/mutations';

// TODO
export function useLogin() {
  return useMutation({
    mutationFn: login,
  });
}
