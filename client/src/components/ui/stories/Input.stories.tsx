import type { Meta, StoryObj } from '@storybook/react-vite';
import { Input } from '../Input';

const meta = {
  title: 'Components/UI/Input',
  component: Input,
  tags: ['autodocs'],
  args: {
    type: 'text',
    placeholder: 'Enter text here...',
  },
} satisfies Meta<typeof Input>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Default: Story = {};

export const Disabled: Story = {
  args: {
    disabled: true,
    placeholder: 'Disabled input',
  },
};

export const Email: Story = {
  args: {
    type: 'email',
    placeholder: 'user@example.com',
  },
};

export const File: Story = {
  args: {
    type: 'file',
  },
};
