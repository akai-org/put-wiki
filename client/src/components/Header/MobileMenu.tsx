import { createPortal } from 'react-dom';
import { Link } from '@tanstack/react-router';
import { SearchBar } from './SearchBar';
import { AuthAction } from './AuthAction';

export function MobileMenu({ onClose }: { onClose: () => void }) {
  return createPortal(
    <div className="md:hidden fixed top-16 left-0 w-full bg-background border-t border-border px-6 py-6 flex flex-col gap-4 shadow-xl z-50">
      <SearchBar
        containerClassName="relative flex items-center gap-2 bg-muted rounded-full pl-2 pr-4 h-11 w-full"
        iconWrapperClassName="bg-background w-10 h-10 rounded-full text-muted-foreground flex items-center justify-center flex-shrink-0"
        inputClassName="w-full bg-transparent text-sm text-foreground placeholder:text-muted-foreground focus:outline-none"
      />

      <nav className="flex flex-col gap-4 font-serif" aria-label="Główna nawigacja">
        <Link
          to="/prowadzacy"
          onClick={onClose}
          className="text-lg text-muted-foreground hover:text-foreground uppercase tracking-wider"
        >
          Prowadzący
        </Link>
        <Link
          to="/przedmioty"
          onClick={onClose}
          className="text-lg text-muted-foreground hover:text-foreground uppercase tracking-wider"
        >
          Przedmioty
        </Link>
      </nav>

      <div className="border-t border-border pt-4 font-serif">
        <AuthAction variant="mobile" onAction={onClose} />
      </div>
    </div>,
    document.body
  );
}
