import type { Meta, StoryObj } from '@storybook/react-vite';
import { Onboarding } from './Onboarding';

const meta = {
  title: 'Features/Onboarding',
  component: Onboarding,
  tags: ['autodocs'],
} satisfies Meta<typeof Onboarding>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Default: Story = {
  decorators: [
    (Story) => {
      localStorage.removeItem('hasSeenOnboarding');
      return <Story />;
    },
  ],
};