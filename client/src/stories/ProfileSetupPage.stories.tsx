import type { Meta, StoryObj } from '@storybook/react-vite';
import ProfileSetupPage from '@/pages/profile-setup';

const meta = {
  title: 'PUT Wiki/Pages/ProfileSetup',
  component: ProfileSetupPage,
  parameters: {
    layout: 'fullscreen',
  },
} satisfies Meta<typeof ProfileSetupPage>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Default: Story = {};
