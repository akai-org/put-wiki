import { expect, test } from 'vitest';

function add(a: number, b: number) {
  return a + b;
}

test('adds 2 + 3 to equal 5', () => {
  expect(add(2, 3)).toBe(5);
});
