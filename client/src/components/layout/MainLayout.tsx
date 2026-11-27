import { Header } from '@/components/layout/header';
import React from 'react';

interface MainLayoutProps {
  children: React.ReactNode;
}

export function MainLayout({ children }: MainLayoutProps) {
  return (
    <div className="flex min-h-dvh flex-col bg-linear-110 from-[hsla(258,22%,66%,1)] via-[hsla(240,42%,69%,1)] to-[hsla(233,51%,37%,1)] to-90%">
      <Header />
      <main className="flex flex-1 flex-col items-center-safe">{children}</main>
      {/* <Footer /> */}
    </div>
  );
}
