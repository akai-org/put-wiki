import type { Decorator } from '@storybook/react-vite';
import { RouterDecorator } from './RouterDecorator';

export const withRouterDecorator: Decorator = (Story, { parameters }) => (
  <RouterDecorator initialPath={parameters.router?.initialPath}>
    <Story />
  </RouterDecorator>
);
