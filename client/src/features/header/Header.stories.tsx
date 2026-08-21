import type { Meta, StoryObj } from '@storybook/react-vite';
import Header from './Header';
import { withRouterDecorator } from '@/tests/utils/withRouterDecorator';
import { AuthProvider } from '@/contexts/AuthProvider';
import { UserProvider } from '@/contexts/UserProvider';
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
    <AuthProvider initialLoggedIn={false}>
      <UserProvider initialNickname="Janek">
        <ThemeProvider initialTheme="light">
          <Header />
        </ThemeProvider>
      </UserProvider>
    </AuthProvider>
  ),
};

export const LoggedInLightMode: Story = {
  render: () => (
    <AuthProvider initialLoggedIn={true}>
      <UserProvider initialNickname="Janek">
        <ThemeProvider initialTheme="light">
          <Header />
        </ThemeProvider>
      </UserProvider>
    </AuthProvider>
  ),
};

export const LoggedOutDarkMode: Story = {
  render: () => (
    <AuthProvider initialLoggedIn={false}>
      <UserProvider initialNickname="Janek">
        <ThemeProvider initialTheme="dark">
          <Header />
        </ThemeProvider>
      </UserProvider>
    </AuthProvider>
  ),
};
