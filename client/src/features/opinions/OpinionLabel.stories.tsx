import type { Meta, StoryObj } from '@storybook/react-vite';
import OpinionLabel from './OpinionLabel';

const meta = {
  title: 'Features/OpinionLabel',
  component: OpinionLabel,
  tags: ['autodocs'],
  args: {
    opinionId: '1',
    userId: 'user-1',
    userName: 'Jan Kowalski',
    content:
      'Bardzo ciekawy kierunek, świetni wykładowcy i mnóstwo praktycznej wiedzy. Zdecydowanie polecam!',
    rating: 5,
  },
} satisfies Meta<typeof OpinionLabel>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Default: Story = {};

export const LowRating: Story = {
  args: {
    opinionId: '2',
    userId: 'user-2',
    userName: 'Anna Nowak',
    content:
      'Niestety, spodziewałam się czegoś innego. Za dużo teorii, za mało zajęć praktycznych.',
    rating: 2,
  },
};

export const ShortOpinion: Story = {
  args: {
    opinionId: '3',
    userId: 'user-3',
    userName: 'Piotr Wiśniewski',
    content: 'Wszystko super.',
    rating: 4,
  },
};
