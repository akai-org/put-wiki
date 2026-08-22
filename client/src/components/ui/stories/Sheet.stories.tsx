import type { Meta, StoryObj } from '@storybook/react-vite';
import {
  Sheet,
  SheetTrigger,
  SheetContent,
  SheetHeader,
  SheetTitle,
  SheetDescription,
  SheetFooter,
} from '../Sheet';
import { Button } from '../Button';
import { Input } from '../Input';

const meta = {
  title: 'Components/UI/Sheet',
  component: Sheet,
  tags: ['autodocs'],
} satisfies Meta<typeof Sheet>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Default: Story = {
  render: () => (
    <Sheet>
      <SheetTrigger asChild>
        <Button variant="outline">Open Sheet</Button>
      </SheetTrigger>
      <SheetContent>
        <SheetHeader>
          <SheetTitle>Edit profile</SheetTitle>
          <SheetDescription>
            Make changes to your profile here. Click save when you are done.
          </SheetDescription>
        </SheetHeader>
        <div className="grid gap-4 py-4">
          <div className="grid grid-cols-4 items-center gap-4">
            <label className="text-right text-sm font-medium" htmlFor="name">
              Name
            </label>
            <Input className="col-span-3" defaultValue="Pedro Duarte" id="name" />
          </div>
        </div>
        <SheetFooter>
          <Button type="submit">Save changes</Button>
        </SheetFooter>
      </SheetContent>
    </Sheet>
  ),
};

export const Sides: Story = {
  render: () => (
    <div className="grid grid-cols-2 gap-2">
      {['top', 'right', 'bottom', 'left'].map((side) => (
        <Sheet key={side}>
          <SheetTrigger asChild>
            <Button className="capitalize" variant="outline">
              {side}
            </Button>
          </SheetTrigger>
          <SheetContent side={side as 'top' | 'right' | 'bottom' | 'left'}>
            <SheetHeader>
              <SheetTitle>Sheet on {side}</SheetTitle>
              <SheetDescription>
                This sheet is sliding in from the {side} edge of the screen.
              </SheetDescription>
            </SheetHeader>
          </SheetContent>
        </Sheet>
      ))}
    </div>
  ),
};
