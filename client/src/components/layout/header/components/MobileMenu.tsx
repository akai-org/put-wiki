import { Link } from '@tanstack/react-router';
import { Sheet, SheetContent, SheetTitle } from '@/components/ui/Sheet';
import { SearchBar } from './SearchBar';
import { AuthAction } from './AuthAction';
import { ThemeToggle } from './ThemeToggle';

export function MobileMenu({ open, onClose }: { open: boolean; onClose: () => void }) {
  return (
    <Sheet onOpenChange={(open) => !open && onClose()} open={open}>
      <SheetContent
        className="inset-x-3 top-16 h-auto gap-4 rounded-b-2xl border px-5 py-6 md:hidden"
        showCloseButton={false}
        side="top"
      >
        <SheetTitle className="sr-only">Menu nawigacyjne</SheetTitle>

        <div className="flex items-center justify-between gap-4 border-b border-border pb-4 font-serif">
          <ThemeToggle />
          <div className="flex-1">
            <AuthAction onAction={onClose} variant="mobile" />
          </div>
        </div>

        <nav aria-label="Główna nawigacja" className="flex flex-col gap-4 font-serif">
          <Link
            className="text-lg text-foreground/85 transition-colors hover:text-foreground"
            onClick={onClose}
            to="/"
          >
            Prowadzący
          </Link>
          <Link
            className="text-lg text-foreground/85 transition-colors hover:text-foreground"
            onClick={onClose}
            to="/"
          >
            Przedmioty
          </Link>
        </nav>

        <SearchBar
          containerClassName="relative flex items-center gap-2 bg-muted rounded-full pl-2 pr-4 h-11 w-full"
          iconWrapperClassName="bg-background w-10 h-10 rounded-full text-muted-foreground flex items-center justify-center flex-shrink-0"
          inputClassName="w-full bg-transparent text-sm text-foreground placeholder:text-muted-foreground focus:outline-none"
        />
      </SheetContent>
    </Sheet>
  );
}
