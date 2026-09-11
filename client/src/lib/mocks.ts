// MSW mocks are enabled by default in development. Set VITE_ENABLE_MOCKS=false in .env.local to use the real backend.
const isMockingEnabled =
  import.meta.env.NODE_ENV === 'development' && import.meta.env.VITE_ENABLE_MOCKS !== 'false';

export { isMockingEnabled };
