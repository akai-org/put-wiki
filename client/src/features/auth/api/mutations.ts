import { api } from '@/lib/api';
import { isMockingEnabled } from '@/lib/mocks';

function getCurrentPath(): string {
  return window.location.pathname + window.location.search;
}

// Login is a full-page redirect flow:
// GET /api/auth/login -> 302 to USOS authorize page -> USOS redirects to GET /api/auth/callback?oauth_token&oauth_verifier
// -> backend sets httpOnly `auth_token` cookie with JWT.
// TODO: backend doesn't redirect back to the client after callback yet, so returnUrl is honored only by mocks.
async function login(returnUrl: string = getCurrentPath()): Promise<void> {
  if (isMockingEnabled) {
    // MSW can't intercept page navigation, so the redirect to USOS is replaced by the fake USOS page (/mock-usos)
    window.location.assign(
      `/mock-usos?${new URLSearchParams({ oauth_token: crypto.randomUUID(), returnUrl })}`
    );
    return;
  }

  window.location.assign('/api/auth/login');
}

// TODO: backend doesn't have logout endpoint yet. httpOnly cookie can be removed only by the server.
async function logout(): Promise<void> {
  await api.post('/api/auth/logout');
}

export { login, logout };
