import '#styles/tailwind.css';
import '#styles/global.css';
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { HomePage, NotFoundPage } from '#pages';
import { createRootRoute, createRoute, createRouter, RouterProvider } from '@tanstack/react-router';

const rootRoute = createRootRoute();
const indexRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/',
  component: () => <HomePage />,
});
const notFoundRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '$',
  component: () => <NotFoundPage />,
});
const routeTree = rootRoute.addChildren([indexRoute, notFoundRoute]);
const router = createRouter({ routeTree });

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <RouterProvider router={router} />
  </StrictMode>
);
