import { http, HttpResponse } from 'msw';

export const handlers = [
  http.get('https://api.example.com/user-test', () => {
    return HttpResponse.json({
      id: 'abc-123',
      firstName: 'John',
      lastName: 'Maverick',
    });
  }),

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
];
