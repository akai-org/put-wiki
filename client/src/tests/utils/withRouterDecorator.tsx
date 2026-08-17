import type { Decorator } from '@storybook/react-vite';
import { RouterDecorator } from './RouterDecorator';

export const withRouterDecorator: Decorator = (Story) => (
  <RouterDecorator>
    <Story />
  </RouterDecorator>
);
