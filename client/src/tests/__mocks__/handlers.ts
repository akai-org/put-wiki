import { http, HttpResponse } from 'msw';

export const handlers = [
  http.get('https://api.example.com/user', () => {
    return HttpResponse.json({
      id: 'abc-123',
      firstName: 'John',
      lastName: 'Maverick',
    });
  }),

  http.get<{ nickname: string }>('/profile/:nickname', ({ params }) => {
    const { nickname } = params;
    return HttpResponse.json({
      nickname: nickname,
      opinions: 10,
      reactions: 5,
      karma: 100,
    });
  }),
];
