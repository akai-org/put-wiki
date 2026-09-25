import type { Meta, StoryObj } from '@storybook/react-vite';
import ConsultationsCard from './../components/ConsultationsCard';

const meta = {
  title: 'Features/Lecturers/ConsultationsCard',
  component: ConsultationsCard,
  tags: ['autodocs'],
} satisfies Meta<typeof ConsultationsCard>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Default: Story = {};
