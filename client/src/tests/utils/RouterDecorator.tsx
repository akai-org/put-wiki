import { createContext, useContext, useState, type ReactNode } from 'react';
import {
  createMemoryHistory,
  createRootRoute,
  createRoute,
  createRouter,
  RouterProvider,
} from '@tanstack/react-router';

const RouterChildrenContext = createContext<ReactNode>(null);

function RouterChildren() {
  return useContext(RouterChildrenContext);
}

function createTestRouter(initialPath: string) {
  const rootRoute = createRootRoute({ component: RouterChildren });
  // Index and splat routes let any initial path (e.g. /courses/abc) match without a not-found state.
  const indexRoute = createRoute({ getParentRoute: () => rootRoute, path: '/' });
  const splatRoute = createRoute({ getParentRoute: () => rootRoute, path: '$' });

  return createRouter({
    routeTree: rootRoute.addChildren([indexRoute, splatRoute]),
    history: createMemoryHistory({ initialEntries: [initialPath] }),
  });
}

export function RouterDecorator({
  children,
  initialPath = '/',
}: {
  children: ReactNode;
  initialPath?: string;
}) {
  const [router] = useState(() => createTestRouter(initialPath));

  return (
    <RouterChildrenContext value={children}>
      <RouterProvider router={router} />
    </RouterChildrenContext>
  );
}
