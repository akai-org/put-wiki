import { MutationCache, QueryCache, QueryClient } from '@tanstack/react-query';
import { isAxiosError } from 'axios';

function isUnauthorizedError(error: unknown): boolean {
  return isAxiosError(error) && error.response?.status === 401;
}

// Session cookie is missing, invalid or expired - refetch user session (['auth'] key from features/auth),
// so the UI switches to logged out state
function handleUnauthorizedError(error: unknown) {
  if (isUnauthorizedError(error)) {
    void queryClient.invalidateQueries({ queryKey: ['auth'] });
  }
}

const queryClient: QueryClient = new QueryClient({
  queryCache: new QueryCache({ onError: handleUnauthorizedError }),
  mutationCache: new MutationCache({ onError: handleUnauthorizedError }),
  defaultOptions: {
    queries: {
      // retrying won't help when user is not logged in
      retry: (failureCount, error) => !isUnauthorizedError(error) && failureCount < 3,
    },
  },
});

export { queryClient };
