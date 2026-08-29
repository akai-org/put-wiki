import { Header } from '@/components/layout/header';
import React from 'react';

interface MainLayoutProps {
  children: React.ReactNode;
}

export function MainLayout({ children }: MainLayoutProps) {
  return (
    <div className="flex min-h-screen flex-col">
      <Header />
      <main className="flex flex-col items-center-safe">{children}</main>
      {/* <Footer /> */}
    </div>
  );
}
