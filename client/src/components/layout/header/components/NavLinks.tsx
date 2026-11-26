import { Link } from '@tanstack/react-router';
import { cn } from '@/utils';
import { focusRing } from '../focusRing';
import { navItems } from '../navItems';

const linkClassName = cn(
  'relative inline-flex rounded-sm py-3 text-base font-medium text-foreground transition-colors',
  'after:absolute after:inset-x-0 after:bottom-1.5 after:h-0.5 after:rounded-full after:transition-colors',
  'hover:after:bg-foreground/40 aria-[current=page]:after:bg-secondary',
  focusRing
);

export function NavLinks({
  orientation = 'horizontal',
  onNavigate,
  className,
}: {
  orientation?: 'horizontal' | 'vertical';
  onNavigate?: () => void;
  className?: string;
}) {
  return (
    <nav aria-label="Główna nawigacja" className={className}>
      <ul
        className={cn(
          'flex',
          orientation === 'horizontal' ? 'items-center gap-5.5' : 'flex-col items-start gap-1'
        )}
      >
        {navItems.map(({ label, to }) => (
          <li key={to}>
            <Link className={linkClassName} onClick={onNavigate} to={to}>
              {label}
            </Link>
          </li>
        ))}
      </ul>
    </nav>
  );
}
