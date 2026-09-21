import axios, { AxiosError } from 'axios';
import type { InternalAxiosRequestConfig } from 'axios';

const agent = axios.create({
  baseURL: '/',
  withCredentials: true,
});

type RetriableRequestConfig = InternalAxiosRequestConfig & {
  _authRetry?: boolean;
};

let refreshPromise: Promise<void> | null = null;

function refreshSession(): Promise<void> {
  refreshPromise ??= agent
    .post('/api/auth/refresh')
    .then(() => undefined)
    .finally(() => {
      refreshPromise = null;
    });

  return refreshPromise;
}

agent.interceptors.response.use(undefined, async (error: AxiosError) => {
  const request = error.config as RetriableRequestConfig | undefined;
  const isRefreshRequest = request?.url?.endsWith('/api/auth/refresh');

  if (error.response?.status !== 401 || !request || request._authRetry || isRefreshRequest) {
    return Promise.reject(error);
  }

  request._authRetry = true;
  await refreshSession();
  return agent(request);
});

export { agent };
