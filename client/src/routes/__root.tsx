import { createRootRoute, Outlet } from '@tanstack/react-router';
import { Header } from '@/components/layout/header';
import NotFoundPage from './-not-found';

export const Route = createRootRoute({
  component: Root,
  notFoundComponent: NotFoundPage,
});

function Root() {
  return (
    <div className="flex min-h-screen flex-col bg-[#121318]">
      <Header />
      <main className="grow">
        <Outlet />
      </main>
      {/* <Footer /> */}
    </div>
  );
}
