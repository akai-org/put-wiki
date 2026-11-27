import { useState } from 'react';
import type { Meta, StoryObj } from '@storybook/react-vite';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuLabel,
  DropdownMenuRadioGroup,
  DropdownMenuRadioItem,
  DropdownMenuTrigger,
} from '../DropdownMenu';
import { Button } from '../Button';

const meta = {
  title: 'Components/UI/DropdownMenu',
  component: DropdownMenu,
  tags: ['autodocs'],
} satisfies Meta<typeof DropdownMenu>;

export default meta;
type Story = StoryObj<typeof meta>;

export const RadioGroup: Story = {
  render: function RadioGroupStory() {
    const [value, setValue] = useState('light');

    return (
      <DropdownMenu>
        <DropdownMenuTrigger asChild>
          <Button variant="outline">Motyw</Button>
        </DropdownMenuTrigger>
        <DropdownMenuContent>
          <DropdownMenuLabel>Motyw</DropdownMenuLabel>
          <DropdownMenuRadioGroup onValueChange={setValue} value={value}>
            <DropdownMenuRadioItem value="light">Jasny</DropdownMenuRadioItem>
            <DropdownMenuRadioItem value="dark">Ciemny</DropdownMenuRadioItem>
          </DropdownMenuRadioGroup>
        </DropdownMenuContent>
      </DropdownMenu>
    );
  },
};
