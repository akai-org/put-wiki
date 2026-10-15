import { defineConfig, mergeConfig } from 'vitest/config';
import viteConfig from './vite.config.ts';

export default mergeConfig(
  viteConfig,
  defineConfig({
    test: {
      name: 'unit',
      include: ['src/**/*.test.{ts,tsx}'],
      exclude: ['.storybook/**', '**/*.stories.{ts,tsx}'],
      environment: 'happy-dom',
      setupFiles: ['vitest.setup.ts'],
    },
  })
);
