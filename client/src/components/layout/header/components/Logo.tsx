import { Globe } from 'lucide-react';
import { Link } from '@tanstack/react-router';

export function Logo({ size }: { size: 'desktop' | 'mobile' }) {
  const isDesktop = size === 'desktop';

  return (
    <Link
      aria-label="PUTwiki - strona główna"
      className="flex items-center gap-2.5 text-foreground transition-opacity hover:opacity-85"
      to="/"
    >
      <Globe aria-hidden="true" className={isDesktop ? 'size-8' : 'size-6'} strokeWidth={2.25} />
      <span
        className={`font-serif leading-none tracking-tight ${isDesktop ? 'text-3xl' : 'text-2xl'}`}
      >
        <span className="font-black">PUT</span>
        <span className="font-normal">wiki</span>
      </span>
    </Link>
  );
}
