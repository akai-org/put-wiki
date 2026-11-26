import { useState } from 'react';
import { Menu, X } from 'lucide-react';
import { Sheet, SheetClose, SheetContent, SheetTitle, SheetTrigger } from '@/components/ui/Sheet';
import { cn } from '@/utils';
import { focusRing } from '../focusRing';
import { AppearanceMenu } from './AppearanceMenu';
import { AuthAction } from './AuthAction';
import { Logo } from './Logo';
import { NavLinks } from './NavLinks';
import { SearchBar } from './SearchBar';

const iconButtonClassName = cn(
  'flex size-10 items-center justify-center rounded-sm text-foreground transition-opacity hover:opacity-85',
  focusRing
);

export function MobileMenu({
  showAuthAction = true,
  className,
}: {
  showAuthAction?: boolean;
  className?: string;
}) {
  const [open, setOpen] = useState(false);
  const close = () => setOpen(false);

  return (
    <Sheet onOpenChange={setOpen} open={open}>
      <SheetTrigger asChild>
        <button
          aria-label="Otwórz menu"
          className={cn(iconButtonClassName, className)}
          type="button"
        >
          <Menu aria-hidden="true" className="size-6" />
        </button>
      </SheetTrigger>

      <SheetContent
        aria-describedby={undefined}
        className="gap-5 rounded-b-2xl border-border bg-background px-8 pb-6 text-foreground shadow-header lg:hidden"
        showCloseButton={false}
        side="top"
      >
        <SheetTitle className="sr-only">Menu nawigacyjne</SheetTitle>

        <div className="flex h-16 items-center justify-between">
          <Logo onNavigate={close} />
          <SheetClose asChild>
            <button aria-label="Zamknij menu" className={iconButtonClassName} type="button">
              <X aria-hidden="true" className="size-6" />
            </button>
          </SheetClose>
        </div>

        <NavLinks onNavigate={close} orientation="vertical" />

        <SearchBar />

        <div className="flex items-center gap-4 border-t border-border pt-4">
          <AppearanceMenu />
          {showAuthAction ? (
            <div className="flex flex-1 justify-end">
              <AuthAction onAction={close} variant="mobile" />
            </div>
          ) : null}
        </div>
      </SheetContent>
    </Sheet>
  );
}
