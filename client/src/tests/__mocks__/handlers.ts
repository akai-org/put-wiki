import { http, HttpResponse } from 'msw';
import { authHandlers } from './authHandlers';

export const handlers = [
  ...authHandlers,
  http.get('https://api.example.com/user', () => {
    return HttpResponse.json({
      id: 'abc-123',
      firstName: 'John',
      lastName: 'Maverick',
    });
  }),
];
