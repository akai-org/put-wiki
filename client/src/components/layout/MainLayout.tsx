import { useRouterState } from '@tanstack/react-router';
import { Header } from '@/components/layout/header';
import React from 'react';

interface MainLayoutProps {
  children: React.ReactNode;
}

const WELCOME_PATH = '/';

export function MainLayout({ children }: MainLayoutProps) {
  const isWelcomePage = useRouterState({
    select: (state) => state.location.pathname === WELCOME_PATH,
  });

  return (
    <div className="flex min-h-screen flex-col">
      {isWelcomePage ? <Header showAuthAction={false} variant="transparent" /> : <Header />}
      <main className="flex flex-col items-center-safe">{children}</main>
      {/* <Footer /> */}
    </div>
  );
}
