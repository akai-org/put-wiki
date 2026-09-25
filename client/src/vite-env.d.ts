/// <reference types="vite/client" />

interface ImportMetaEnv {
  readonly VITE_ENABLE_MOCKS?: string;
  readonly VITE_MOCK_TOKEN_TTL_MINUTES?: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
