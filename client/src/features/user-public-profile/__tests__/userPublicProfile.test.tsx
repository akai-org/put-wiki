import { test, expect } from 'vitest';

test('responds with a public profile by nickname slug', async () => {
  const response = await fetch('/profiles/lolek');

  await expect(response.json()).resolves.toEqual({
    nickname: 'Lolek',
    joinedDate: '2022-01-01',
    userStatus: 'Active',
    opinions: 10,
    reactions: 5,
    karma: 100,
  });
});
