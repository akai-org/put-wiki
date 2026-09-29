import { http, HttpResponse } from 'msw';
import { lecturersHandlers } from './lecturersHandlers';

export const handlers = [
  http.get('https://api.example.com/user', () => {
    return HttpResponse.json({
      id: 'abc-123',
      firstName: 'John',
      lastName: 'Maverick',
    });
  }),

  ...lecturersHandlers,
];
