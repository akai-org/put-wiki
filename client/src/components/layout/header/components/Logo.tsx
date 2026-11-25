import { Link } from '@tanstack/react-router';
import { cn } from '@/utils';
import { focusRing } from '../focusRing';

export function Logo({ onNavigate }: { onNavigate?: () => void }) {
  return (
    <Link
      aria-label="PUTWiki – strona główna"
      className={cn(
        'flex shrink-0 items-center gap-2.5 rounded-sm text-foreground transition-opacity hover:opacity-85',
        focusRing
      )}
      onClick={onNavigate}
      to="/"
    >
      <span
        aria-hidden="true"
        className="flex size-9 items-center justify-center rounded-logo bg-brand text-logo-mark font-extrabold text-brand-foreground"
      >
        p.
      </span>
      <span className="text-2xl leading-tight font-medium">PUTWiki</span>
    </Link>
  );
}
