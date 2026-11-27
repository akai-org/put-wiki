import { http, HttpResponse } from 'msw';

export const mockUser = { nickname: 'Lolek' };

/**
 * Session handlers for tests and stories. Not registered globally,
 * so the dev server keeps hitting the real `/users/me` endpoint.
 */
export const authHandlers = {
  loggedIn: [http.get('/users/me', () => HttpResponse.json(mockUser))],
  loggedOut: [
    http.get('/users/me', () => HttpResponse.json({ message: 'Unauthorized' }, { status: 401 })),
  ],
};
