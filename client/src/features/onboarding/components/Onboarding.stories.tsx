import type { Meta, StoryObj } from '@storybook/react-vite';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { Onboarding } from './Onboarding';

const queryClient = new QueryClient();

const meta = {
  title: 'Features/Onboarding',
  component: Onboarding,
  tags: ['autodocs'],
  decorators: [
    (Story) => {
      localStorage.removeItem('hasSeenOnboarding');
      return (
        <QueryClientProvider client={queryClient}>
          <Story />
        </QueryClientProvider>
      );
    },
  ],
} satisfies Meta<typeof Onboarding>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Default: Story = {};