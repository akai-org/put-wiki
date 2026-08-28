import type { Meta, StoryObj } from '@storybook/react-vite';
import { ScrollArea } from '../ScrollArea';

const meta = {
  title: 'Components/UI/ScrollArea',
  component: ScrollArea,
  tags: ['autodocs'],
} satisfies Meta<typeof ScrollArea>;

export default meta;
type Story = StoryObj<typeof meta>;

const tags = Array.from({ length: 50 }).map((_, i, a) => `v1.2.0-beta.${a.length - i}`);

export const Default: Story = {
  render: () => (
    <ScrollArea className="h-72 w-48 rounded-md border">
      <div className="p-4">
        <h4 className="mb-4 text-sm leading-none font-medium">Tags</h4>
        {tags.map((tag) => (
          <div className="text-sm" key={tag}>
            {tag}
            <div className="my-2 border-t" />
          </div>
        ))}
      </div>
    </ScrollArea>
  ),
};
