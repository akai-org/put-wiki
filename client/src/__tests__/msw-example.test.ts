import { test, expect } from 'vitest';

test('responds with the user', async () => {
  const response = await fetch('https://api.example.com/user-test');

  await expect(response.json()).resolves.toEqual({
    id: 'abc-123',
    firstName: 'John',
    lastName: 'Maverick',
  });
});

test('responds with a public profile by nickname slug', async () => {
  const response = await fetch('/user/lolek');

  await expect(response.json()).resolves.toEqual({
    nickname: 'Lolek',
    joinedDate: '2022-01-01',
    userStatus: 'Active',
    opinions: 10,
    reactions: 5,
    karma: 100,
  });
});
