import type { Meta, StoryObj } from '@storybook/react-vite';
import TableOfOpinions from './TableOfOpinions';

const meta = {
  title: 'Features/TableOfOpinions',
  component: TableOfOpinions,
  tags: ['autodocs'],
} satisfies Meta<typeof TableOfOpinions>;

export default meta;

type Story = StoryObj<typeof meta>;

const mockOpinions = [
  {
    opinionId: '1',
    userId: 'u1',
    userName: 'Jan Kowalski',
    content: 'Świetny kierunek! Wiele się można nauczyć.',
    rating: 5,
  },
  {
    opinionId: '2',
    userId: 'u2',
    userName: 'Anna Nowak',
    content: 'Wymagający, ale warto.',
    rating: 4,
  },
  {
    opinionId: '3',
    userId: 'u3',
    userName: 'Piotr Wiśniewski',
    content: 'Dużo nauki, ciekawe laby. Prowadzący bardzo pomocni.',
    rating: 4,
  },
  {
    opinionId: '4',
    userId: 'u4',
    userName: 'Katarzyna Ząb',
    content: 'Polecam każdemu kto interesuje się tym tematem.',
    rating: 5,
  },
];

export const DefaultWithCollapsible: Story = {
  args: {
    opinions: mockOpinions,
  },
};

export const FewOpinions: Story = {
  args: {
    opinions: mockOpinions.slice(0, 2),
  },
};

export const Empty: Story = {
  args: {
    opinions: [],
  },
};
