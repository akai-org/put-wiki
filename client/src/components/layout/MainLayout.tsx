import { Header } from '@/components/layout/header';
import { Onboarding } from '@/components/layout/onboarding/Onboarding';
import React from 'react';

interface MainLayoutProps {
  children: React.ReactNode;
}

export function MainLayout({ children }: MainLayoutProps) {
  return (
    <div className="flex min-h-screen flex-col">
      <Header />
      <Onboarding />
      <main className="flex flex-col items-center-safe">{children}</main>
      {/* <Footer /> */}
    </div>
  );
}