import type { Meta, StoryObj } from '@storybook/react-vite';
import Header from './Header';
import { withRouterDecorator } from '../../../.storybook/withRouterDecorator';
import { AuthProvider } from '@/contexts/AuthProvider';
import { UserProvider } from '@/contexts/UserProvider';
import { ThemeProvider } from '@/contexts/ThemeProvider';
import type { Theme } from '@/contexts/ThemeContext';

function HeaderWithProviders({
  initialLoggedIn,
  initialTheme,
}: {
  initialLoggedIn?: boolean;
  initialTheme?: Theme;
}) {
  return (
    <AuthProvider initialLoggedIn={initialLoggedIn}>
      <UserProvider>
        <ThemeProvider initialTheme={initialTheme}>
          <Header />
        </ThemeProvider>
      </UserProvider>
    </AuthProvider>
  );
}

const meta = {
  title: 'Components/Header',
  component: HeaderWithProviders,
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [withRouterDecorator],
} satisfies Meta<typeof HeaderWithProviders>;

export default meta;
type Story = StoryObj<typeof meta>;

export const LoggedOutLightMode: Story = {
  args: { initialLoggedIn: false, initialTheme: 'light' },
};

export const LoggedInLightMode: Story = {
  args: { initialLoggedIn: true, initialTheme: 'light' },
};

export const LoggedOutDarkMode: Story = {
  args: { initialLoggedIn: false, initialTheme: 'dark' },
};
