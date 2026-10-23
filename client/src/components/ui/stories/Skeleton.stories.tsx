import type { Meta, StoryObj } from '@storybook/react-vite';
import { Skeleton } from '../Skeleton';

const meta = {
  title: 'Components/UI/Skeleton',
  component: Skeleton,
  tags: ['autodocs'],
  args: {
    className: 'h-4 w-[250px]',
  },
} satisfies Meta<typeof Skeleton>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Default: Story = {};

export const Text: Story = {
  render: () => (
    <div className="flex w-[250px] flex-col gap-2">
      <Skeleton className="h-4 w-full" />
      <Skeleton className="h-4 w-4/5" />
      <Skeleton className="h-4 w-3/5" />
    </div>
  ),
};

export const Card: Story = {
  render: () => (
    <div className="flex w-[350px] flex-col gap-4 rounded-lg border p-4">
      <div className="flex items-center gap-4">
        <Skeleton className="size-12 rounded-full" />
        <div className="flex flex-1 flex-col gap-2">
          <Skeleton className="h-4 w-3/5" />
          <Skeleton className="h-4 w-2/5" />
        </div>
      </div>
      <Skeleton className="h-32 w-full" />
      <div className="flex justify-end gap-2">
        <Skeleton className="h-9 w-20" />
        <Skeleton className="h-9 w-24" />
      </div>
    </div>
  ),
};
