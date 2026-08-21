import type { Meta, StoryObj } from '@storybook/react-vite';
import TableOfSubjects from './TableOfSubjects';

const meta = {
  title: 'Features/TableOfSubjects',
  component: TableOfSubjects,
  tags: ['autodocs'],
} satisfies Meta<typeof TableOfSubjects>;

export default meta;
type Story = StoryObj<typeof meta>;

const mockSemesters = [
  {
    number: 1,
    subjects: ['Matematyka I', 'Fizyka', 'Wstęp do programowania', 'BHP i Ergonomia'],
  },
  {
    number: 2,
    subjects: ['Matematyka II', 'Architektura komputerów', 'Algorytmy i struktury danych'],
  },
  {
    number: 3,
    subjects: ['Bazy danych', 'Sieci komputerowe', 'Programowanie obiektowe', 'Systemy operacyjne'],
  },
  {
    number: 4,
    subjects: ['Inżynieria oprogramowania', 'Programowanie aplikacji internetowych'],
  },
];

export const Default: Story = {
  args: {
    semesters: mockSemesters,
  },
};

export const SingleSemester: Story = {
  args: {
    semesters: [mockSemesters[0]],
  },
};

export const Empty: Story = {
  args: {
    semesters: [],
  },
};
