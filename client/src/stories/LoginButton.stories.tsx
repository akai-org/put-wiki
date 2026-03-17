import type { Meta, StoryObj } from '@storybook/react-vite';
import LoginButton from '@/components/LoginButton';

const meta = {
  title: 'PUT Wiki/Components/LoginButton',
  component: LoginButton,
  parameters: {
    layout: 'centered',
  },
} satisfies Meta<typeof LoginButton>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Default: Story = {};
