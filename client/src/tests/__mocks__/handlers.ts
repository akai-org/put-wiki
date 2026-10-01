import { http, HttpResponse } from 'msw';
import { lecturersHandlers } from './lecturersHandlers';

export const handlers = [
  http.get<{ nickname: string }>('/user/:nickname', ({ params }) => {
    const { nickname } = params;
    return HttpResponse.json({
      nickname: nickname,
      joinedDate: '2022-01-01',
      userStatus: 'Active',
      opinions: 10,
      reactions: 5,
      karma: 100,
    });
  }),
  ...lecturersHandlers,
];
