import { beforeAll, afterEach, afterAll } from 'vitest';
import { cleanup } from '@testing-library/react';
import '@testing-library/jest-dom/vitest';

import { server } from './src/tests/__mocks__/node';

beforeAll(() => server.listen());
afterEach(() => {
  // Vitest runs without globals, so RTL cannot register its automatic cleanup.
  cleanup();
  server.resetHandlers();
});
afterAll(() => server.close());
