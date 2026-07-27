import type { Meta, StoryObj } from '@storybook/react-vite';
import Header from './Header';
import { withRouterDecorator } from '../../../.storybook/withRouterDecorator';
import { withProviders } from '../../../.storybook/withProviders';

const meta = {
  title: 'Components/Header',
  component: Header,
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [withRouterDecorator],
} satisfies Meta<typeof Header>;

export default meta;
type Story = StoryObj<typeof meta>;

export const LoggedOutLightMode: Story = {
  decorators: [withProviders({ initialLoggedIn: false, initialDark: false })],
};

export const LoggedInLightMode: Story = {
  decorators: [withProviders({ initialLoggedIn: true, initialDark: false })],
};

export const LoggedOutDarkMode: Story = {
  decorators: [withProviders({ initialLoggedIn: false, initialDark: true })],
};
