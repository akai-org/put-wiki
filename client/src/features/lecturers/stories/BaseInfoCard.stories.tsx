import type { Meta, StoryObj } from '@storybook/react-vite';
import BaseInfoCard from './../components/BaseInfoCard';

const meta = {
  title: 'Features/Lecturers/BaseInfoCard',
  component: BaseInfoCard,
  tags: ['autodocs'],
} satisfies Meta<typeof BaseInfoCard>;

export default meta;
type Story = StoryObj<typeof meta>;

const mockBaseInfo = {
  name: 'Jan Kowalski',
  title: 'Professor',
  photoUrl: 'https://placehold.co/200x200',
};

export const Default: Story = { args: mockBaseInfo };
