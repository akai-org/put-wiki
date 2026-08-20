import type { Meta, StoryObj } from '@storybook/react-vite';
import ToasterPage from '@/tests/ToastsTestPage';
import { ThemeProvider } from '@/contexts/ThemeProvider';
import { Toaster } from '@/components/ui/Toaster';

const meta = {
  title: 'Test/ToastsTestPage',
  component: ToasterPage,
  decorators: [
    (Story) => (
      <ThemeProvider initialTheme="light">
        <Toaster position="top-center" />
        <Story />
      </ThemeProvider>
    ),
  ],
} satisfies Meta<typeof ToasterPage>;

export default meta;
type Story = StoryObj<typeof meta>;

export const ToastersPage: Story = {};
