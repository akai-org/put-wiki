import type { Meta, StoryObj } from '@storybook/react-vite';
import Toggle from '../Toggle';

const meta = {
  title: 'Components/UI/Toggle',
  component: Toggle,
  tags: ['autodocs'],
  args: {
    onValue: 'On',
    offValue: 'Off',
    variant: 'default',
    size: 'default',
  },
  argTypes: {
    variant: {
      control: 'select',
      options: ['default', 'destructive', 'outline', 'secondary', 'ghost', 'link'],
    },
    size: {
      control: 'select',
      options: ['default', 'xs', 'sm', 'lg', 'icon', 'icon-xs', 'icon-sm', 'icon-lg'],
    },
  },
} satisfies Meta<typeof Toggle>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Default: Story = {};

export const CustomValues: Story = {
  args: {
    onValue: 'Subscribed',
    offValue: 'Subscribe',
    variant: 'secondary',
  },
};

export const OutlineVariant: Story = {
  args: {
    onValue: 'Enabled',
    offValue: 'Disabled',
    variant: 'outline',
  },
};
