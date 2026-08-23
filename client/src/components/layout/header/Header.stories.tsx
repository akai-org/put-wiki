import type { Meta, StoryObj } from '@storybook/react-vite';
import Header from './Header';
import { withRouterDecorator } from '@/tests/utils/withRouterDecorator';
import { ThemeProvider } from '@/contexts/ThemeProvider';

const meta = {
  title: 'Features/Header',
  component: Header,
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [withRouterDecorator],
} satisfies Meta<typeof Header>;

export default meta;
type Story = StoryObj<typeof meta>;

export const LoggedOutLightMode: Story = {
  render: () => (
    <ThemeProvider initialTheme="light">
      <Header />
    </ThemeProvider>
  ),
};

export const LoggedInLightMode: Story = {
  render: () => (
    <ThemeProvider initialTheme="light">
      <Header />
    </ThemeProvider>
  ),
};

export const LoggedOutDarkMode: Story = {
  render: () => (
    <ThemeProvider initialTheme="dark">
      <Header />
    </ThemeProvider>
  ),
};
