import { useState } from 'react';
import { Menu, X } from 'lucide-react';
import { Link } from '@tanstack/react-router';
import { SearchBar } from '@/features/header/components/SearchBar';
import { AuthAction } from '@/features/header/components/AuthAction';
import { MobileMenu } from '@/features/header/components/MobileMenu';
import { ThemeToggle } from '@/features/header/components/ThemeToggle';
import { Logo } from '@/features/header/components/Logo';

const navLinkClassName =
  'text-[17px] font-serif font-normal text-foreground/85 hover:text-foreground transition-colors whitespace-nowrap';

export default function Header() {
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);

  return (
    <header className="sticky top-0 z-50 w-full border-b border-border bg-background text-foreground shadow-md select-none">
      <div className="flex w-full justify-center px-8 md:px-16">
        <nav
          aria-label="Główna nawigacja"
          className="hidden h-20 w-full max-w-300 grid-cols-[auto_1fr_auto] items-center gap-4 md:grid"
        >
          <div className="flex items-center justify-start">
            <Logo size="desktop" />
          </div>

          <div className="flex w-full justify-center">
            <SearchBar
              containerClassName="relative flex items-center gap-3.5 bg-muted rounded-full pl-2.5 pr-4 h-12 w-full max-w-[340px] border border-input focus-within:border-ring transition-all"
              iconWrapperClassName="bg-background w-10 h-10 rounded-full text-muted-foreground flex items-center justify-center shadow-inner flex-shrink-0"
              inputClassName="w-full bg-transparent text-[15px] text-foreground font-serif placeholder:text-muted-foreground focus:outline-none"
            />
          </div>

          <div className="flex items-center justify-end gap-8">
            <Link className={navLinkClassName} to="/">
              Prowadzący
            </Link>
            <Link className={navLinkClassName} to="/">
              Przedmioty
            </Link>

            <div className="flex min-w-10 items-center justify-end gap-4 pl-2">
              <ThemeToggle />
              <AuthAction variant="desktop" />
            </div>
          </div>
        </nav>
      </div>

      <div className="flex h-16 w-full items-center justify-between px-8 md:hidden">
        <Logo size="mobile" />

        <button
          className="p-2 text-muted-foreground hover:text-foreground focus:outline-none"
          onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
          type="button"
        >
          {isMobileMenuOpen ? <X className="size-6" /> : <Menu className="size-6" />}
        </button>
      </div>

      <MobileMenu onClose={() => setIsMobileMenuOpen(false)} open={isMobileMenuOpen} />
    </header>
  );
}
