import { afterEach, beforeEach, expect, test, vi } from 'vitest';

const user = {
  id: 'abc-123',
  firstName: 'John',
  lastName: 'Maverick',
};

beforeEach(() => {
  vi.stubGlobal(
    'fetch',
    vi.fn(
      async () =>
        new Response(JSON.stringify(user), {
          headers: { 'Content-Type': 'application/json' },
        })
    )
  );
});

afterEach(() => vi.unstubAllGlobals());

test('responds with the user', async () => {
  const response = await fetch('https://api.example.com/user');

  await expect(response.json()).resolves.toEqual(user);
});
