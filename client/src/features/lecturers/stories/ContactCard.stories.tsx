import type { Meta, StoryObj } from '@storybook/react-vite';
import ContactCard from './../components/ContactCard';

const meta = {
  title: 'Features/Lecturers/ContactCard',
  component: ContactCard,
  tags: ['autodocs'],
} satisfies Meta<typeof ContactCard>;

export default meta;
type Story = StoryObj<typeof meta>;

const mockContactInfo = {
  email: 'contact@example.com',
  phone: '+48 123 456 789',
  websiteUrl: 'https://example.com',
};

export const Default: Story = {
  args: { ...mockContactInfo },
};
