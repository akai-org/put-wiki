// MSW mocks are enabled by default in development. Set VITE_ENABLE_MOCKS=false in .env.local to use the real backend.
const isMockingEnabled = import.meta.env.DEV && import.meta.env.VITE_ENABLE_MOCKS !== 'false';

export { isMockingEnabled };
