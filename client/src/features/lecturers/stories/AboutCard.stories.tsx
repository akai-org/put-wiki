import type { Meta, StoryObj } from '@storybook/react-vite';
import AboutCard from './../components/AboutCard';

const meta = {
  title: 'Features/Lecturers/AboutCard',
  component: AboutCard,
  tags: ['autodocs'],
} satisfies Meta<typeof AboutCard>;

export default meta;
type Story = StoryObj<typeof meta>;

const mockDescription =
  'Wymagający prowadzący, piszcie do niego maile grupowo bo nie odpowiada na indywidualne.';

export const Default: Story = {
  args: { description: mockDescription },
};
