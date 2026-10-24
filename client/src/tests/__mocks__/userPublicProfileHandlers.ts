import { http, HttpResponse } from 'msw';

export const userPublicProfileHandlers = [
  http.get<{ nicknameSlug: string }>('/profiles/:nicknameSlug', () => {
    return HttpResponse.json({
      nickname: 'Lolek',
      joinedDate: '2022-01-01',
      userStatus: 'Aktywny',
      opinions: 10,
      reactions: 5,
      karma: 100,
    });
  }),
];
