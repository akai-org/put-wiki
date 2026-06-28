import type { Meta, StoryObj } from '@storybook/react-vite';

import LecturerPage from '@/pages/lecturer';

const meta = {
  title: 'PUT Wiki/Lecturer Page',
  component: LecturerPage,
  parameters: {
    layout: 'fullscreen',
  },
} satisfies Meta<typeof LecturerPage>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Default: Story = {};
