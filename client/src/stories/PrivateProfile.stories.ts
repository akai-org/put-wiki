import type { Meta, StoryObj } from '@storybook/react-vite';
import Page from '@/pages/private-profile';

const meta = {
  title: 'PUT Wiki/Private Profile',
  component: Page,
  parameters: {
    layout: 'fullscreen',
  },
} satisfies Meta<typeof Page>;

export default meta;

type Story = StoryObj<typeof meta>;

export const Default: Story = {};
