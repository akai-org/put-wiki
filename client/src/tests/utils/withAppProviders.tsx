import type { Decorator } from '@storybook/react-vite';
import { AppProviders } from './AppProviders';

/**
 * Wraps a story in theme, React Query and router providers.
 * Configure with `parameters.theme` and `parameters.router.initialPath`.
 */
export const withAppProviders: Decorator = (Story, { parameters }) => (
  <AppProviders initialPath={parameters.router?.initialPath} theme={parameters.theme}>
    <Story />
  </AppProviders>
);
