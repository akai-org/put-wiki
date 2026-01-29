import type { Meta, StoryObj } from '@storybook/react-vite';

import Page from '@/pages/home';

const meta = {
  title: 'PUT Wiki/Home Page',
  component: Page,
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/configure/story-layout
    layout: 'fullscreen',
  },
} satisfies Meta<typeof Page>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Default: Story = {};
