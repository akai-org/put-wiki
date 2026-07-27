import React from 'react';
import type { Decorator } from '@storybook/react-vite';
import {
  createRootRoute,
  createRouter,
  RouterProvider,
  createMemoryHistory,
} from '@tanstack/react-router';

export const withRouterDecorator: Decorator = (Story) => {
  const rootRoute = createRootRoute({
    component: Story,
  });

  const memoryHistory = createMemoryHistory({
    initialEntries: ['/'],
  });

  const mockRouter = createRouter({
    routeTree: rootRoute,
    history: memoryHistory,
  });

  return React.createElement(RouterProvider, { router: mockRouter });
};
