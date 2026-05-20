import type { Meta, StoryObj } from '@storybook/react-vite';
import DegreeCourse from '../pages/degree_course';

const meta = {
  title: 'Pages/DegreeCourse',
  component: DegreeCourse,
} satisfies Meta<typeof DegreeCourse>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Primary: Story = {
  args: {},
};
