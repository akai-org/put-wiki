import type { Meta, StoryObj } from '@storybook/react-vite';
import {
  createMemoryHistory,
  createRootRoute,
  createRoute,
  createRouter,
  RouterProvider,
} from '@tanstack/react-router';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

import LecturerPage from '@/pages/lecturer';

const rootRoute = createRootRoute();

const lecturerRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/lecturers/$slug',
  component: LecturerPage,
});

const routeTree = rootRoute.addChildren([lecturerRoute]);

const meta = {
  title: 'PUT Wiki/Lecturer Page',
  component: LecturerPage,
  args: {
    slug: 'jan-kowalski',
  },
  argTypes: {
    slug: {
      control: 'text',
    },
  },
} satisfies Meta<{ slug: string }>;

export default meta;

type Story = StoryObj<{ slug: string }>;

export const Default: Story = {
  render: (args) => {
    const { slug } = args;
    const queryClient = new QueryClient({
      defaultOptions: {
        queries: {
          retry: false,
        },
      },
    });

    const router = createRouter({
      routeTree,
      history: createMemoryHistory({
        initialEntries: [`/lecturers/${slug}`],
      }),
    });

    return (
      <QueryClientProvider client={queryClient}>
        <RouterProvider router={router} />
      </QueryClientProvider>
    );
  },
};
