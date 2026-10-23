import { api } from '@/lib/api';
import { test, expect } from 'vitest';

test('responds with a public profile by nickname slug', async () => {
  const response = await api.get('/profiles/lolek');

  expect(response.data).toMatchObject({
    nickname: 'Lolek',
    joinedDate: '2022-01-01',
    userStatus: 'Aktywny',
    opinions: 10,
    reactions: 5,
    karma: 100,
  });
});
