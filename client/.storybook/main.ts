import type { StorybookConfig } from '@storybook/react-vite';
import { mergeConfig } from 'vite';
import { msw } from 'msw/vite';

const config: StorybookConfig = {
  stories: ['../src/**/*.stories.@(ts|tsx)'],
  addons: [
    '@chromatic-com/storybook',
    '@storybook/addon-vitest',
    '@storybook/addon-a11y',
    '@storybook/addon-docs',
    'msw-storybook-addon',
  ],
  framework: '@storybook/react-vite',
  viteFinal: async (config) =>
    mergeConfig(config, {
      plugins: [msw({ mode: 'worker-only' })],
    }),
};
export default config;
