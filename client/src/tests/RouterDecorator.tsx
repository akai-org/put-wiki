import type { ReactNode } from 'react';
import {
  createRootRoute,
  createRouter,
  RouterProvider,
  createMemoryHistory,
} from '@tanstack/react-router';

export function RouterDecorator({ children }: { children: ReactNode }) {
  const rootRoute = createRootRoute({
    component: () => children,
  });

  
  const memoryHistory = createMemoryHistory({
    initialEntries: ['/'],
  });

  const mockRouter = createRouter({
    routeTree: rootRoute,
    history: memoryHistory,
  });

  return <RouterProvider router={mockRouter} />;
}
