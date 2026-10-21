import { http, HttpResponse } from 'msw';
import { lecturersHandlers } from './lecturersHandlers';
import { coursesHandlers } from './coursesHandlers';
import { degreeCoursesHandlers } from './degreeCoursesHandlers';

export const handlers = [
  http.get('https://api.example.com/user-test', () => {
    return HttpResponse.json({
      id: 'abc-123',
      firstName: 'John',
      lastName: 'Maverick',
    });
  }),

  http.get<{ nicknameSlug: string }>('*/profiles/:nicknameSlug', () => {
    return HttpResponse.json({
      nickname: 'Lolek',
      joinedDate: '2022-01-01',
      userStatus: 'Active',
      opinions: 10,
      reactions: 5,
      karma: 100,
    });
  }),
  ...lecturersHandlers,
  ...coursesHandlers,
  ...degreeCoursesHandlers,
];
