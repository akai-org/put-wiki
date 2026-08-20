import { createRootRoute, Outlet } from '@tanstack/react-router';
import Header from '@/components/header/Header';
import NotFoundPage from './-not-found';

export const Route = createRootRoute({
  component: Root,
  notFoundComponent: NotFoundPage,
});

function Root() {
  return (
    <div className="flex flex-col min-h-screen bg-[#121318]">
      <Header />
      <main className="grow">
        <Outlet />
      </main>
      {/* <Footer /> */}
    </div>
  );
}
