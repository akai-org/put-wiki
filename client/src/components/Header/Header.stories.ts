import type { Meta, StoryObj } from '@storybook/react-vite';
import React from 'react';
import Header from './Header';
import {
  createRootRoute,
  createRouter,
  RouterProvider,
  createMemoryHistory,
} from '@tanstack/react-router';

const meta = {
  title: 'Components/Header',
  component: Header,
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    (Story) => {
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
    },
  ],
} satisfies Meta<typeof Header>;

export default meta;
type Story = StoryObj<typeof meta>;

// Historia 1: Wylogowany (Jasny motyw)
export const LoggedOut: Story = {
  args: {
    defaultLoggedIn: false,
    defaultDark: false,
  },
};

// Historia 2: Zalogowany (Jasny motyw)
export const LoggedIn: Story = {
  args: {
    defaultLoggedIn: true,
    defaultDark: false,
  },
};

// Historia 3: Tryb Ciemny (Wylogowany)
export const DarkMode: Story = {
  args: {
    defaultLoggedIn: false,
    defaultDark: true,
  },
};
