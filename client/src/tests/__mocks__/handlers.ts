import { http, HttpResponse } from 'msw';
import { lecturersHandlers } from './lecturersHandlers';
import { coursesHandlers } from './coursesHandlers';
import { degreeCoursesHandlers } from './degreeCoursesHandlers';
import { userPublicProfileHandlers } from './userPublicProfileHandlers';

export const handlers = [
  http.get('https://api.example.com/user-test', () => {
    return HttpResponse.json({
      id: 'abc-123',
      firstName: 'John',
      lastName: 'Maverick',
    });
  }),

  ...userPublicProfileHandlers,
  ...lecturersHandlers,
  ...coursesHandlers,
  ...degreeCoursesHandlers,
];
