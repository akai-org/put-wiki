import { Link } from '@tanstack/react-router';
import { Sheet, SheetContent, SheetTitle } from '@/components/ui/sheet';
import { SearchBar } from './SearchBar';
import { AuthAction } from './AuthAction';
import { ThemeToggle } from './ThemeToggle';

export function MobileMenu({ open, onClose }: { open: boolean; onClose: () => void }) {
  return (
    <Sheet open={open} onOpenChange={(open) => !open && onClose()}>
      <SheetContent
        side="top"
        showCloseButton={false}
        className="md:hidden top-16 h-auto border-t px-6 py-6 gap-4"
      >
        <SheetTitle className="sr-only">Menu nawigacyjne</SheetTitle>

        <SearchBar
          containerClassName="relative flex items-center gap-2 bg-muted rounded-full pl-2 pr-4 h-11 w-full"
          iconWrapperClassName="bg-background w-10 h-10 rounded-full text-muted-foreground flex items-center justify-center flex-shrink-0"
          inputClassName="w-full bg-transparent text-sm text-foreground placeholder:text-muted-foreground focus:outline-none"
        />

        <nav className="flex flex-col gap-4 font-serif" aria-label="Główna nawigacja">
          <Link
            to="/prowadzacy"
            onClick={onClose}
            className="text-lg text-foreground/85 hover:text-foreground transition-colors"
          >
            Prowadzący
          </Link>
          <Link
            to="/przedmioty"
            onClick={onClose}
            className="text-lg text-foreground/85 hover:text-foreground transition-colors"
          >
            Przedmioty
          </Link>
        </nav>

        <div className="border-t border-border pt-4 font-serif flex items-center justify-between gap-4">
          <ThemeToggle />
          <div className="flex-1">
            <AuthAction variant="mobile" onAction={onClose} />
          </div>
        </div>
      </SheetContent>
    </Sheet>
  );
}
