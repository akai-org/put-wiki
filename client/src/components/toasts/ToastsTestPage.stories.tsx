import type { Meta, StoryObj } from '@storybook/react-vite';
import ToasterPage from '@/components/toasts/ToastsTestPage';
import { ThemeProvider } from '@/contexts/ThemeProvider';
import { ToastsProvider } from '@/components/ui/ToastsProvider';

const meta = {
  title: 'Components/Toasts',
  component: ToasterPage,
  decorators: [
    (Story) => (
      <ThemeProvider initialTheme="light">
        <ToastsProvider position="top-center" />
        <Story />
      </ThemeProvider>
    ),
  ],
} satisfies Meta<typeof ToasterPage>;

export default meta;
type Story = StoryObj<typeof meta>;

export const ToastersPage: Story = {};
