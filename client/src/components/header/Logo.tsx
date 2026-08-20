import { Globe } from 'lucide-react';
import { Link } from '@tanstack/react-router';

export function Logo({ size }: { size: 'desktop' | 'mobile' }) {
  const isDesktop = size === 'desktop';

  return (
    <Link
      aria-label="PUTwiki - strona główna"
      className="flex items-center gap-2.5 text-foreground hover:opacity-85 transition-opacity"
      to="/"
    >
      <Globe aria-hidden="true" className={isDesktop ? 'h-8 w-8' : 'h-6 w-6'} strokeWidth={2.25} />
      <span
        className={`font-serif tracking-tight leading-none ${isDesktop ? 'text-3xl' : 'text-2xl'}`}
      >
        <span className="font-black">PUT</span>
        <span className="font-normal">wiki</span>
      </span>
    </Link>
  );
}
