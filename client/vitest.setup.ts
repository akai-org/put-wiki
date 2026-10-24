import { beforeAll, afterEach, afterAll } from 'vitest';
import '@testing-library/jest-dom/vitest';

import { server } from './src/tests/__mocks__/node';

beforeAll(() => server.listen());
afterEach(() => server.resetHandlers());
afterAll(() => server.close());
