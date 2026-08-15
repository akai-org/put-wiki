import { useState } from 'react';
import { Menu, X } from 'lucide-react';
import { Link } from '@tanstack/react-router';
import { SearchBar } from './SearchBar';
import { AuthAction } from './AuthAction';
import { MobileMenu } from './MobileMenu';
import { ThemeToggle } from './ThemeToggle';
import { Logo } from './Logo';

const navLinkClassName =
  'text-[17px] font-serif font-normal text-foreground/85 hover:text-foreground transition-colors whitespace-nowrap';

export default function Header() {
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);

  return (
    <header className="sticky top-0 z-50 w-full bg-background text-foreground border-b border-border shadow-md select-none">
      <div className="w-full flex justify-center px-8 md:px-16">
        <nav
          aria-label="Główna nawigacja"
          className="w-full max-w-[1200px] h-20 hidden md:grid grid-cols-[auto_1fr_auto] items-center gap-4"
        >
          <div className="flex items-center justify-start">
            <Logo size="desktop" />
          </div>

          <div className="flex justify-center w-full">
            <SearchBar
              containerClassName="relative flex items-center gap-3.5 bg-muted rounded-full pl-2.5 pr-4 h-12 w-full max-w-[340px] border border-input focus-within:border-ring transition-all"
              iconWrapperClassName="bg-background w-10 h-10 rounded-full text-muted-foreground flex items-center justify-center shadow-inner flex-shrink-0"
              inputClassName="w-full bg-transparent text-[15px] text-foreground font-serif placeholder:text-muted-foreground focus:outline-none"
            />
          </div>

          <div className="flex items-center gap-8 justify-end">
            <Link to="/prowadzacy" className={navLinkClassName}>
              Prowadzący
            </Link>
            <Link to="/przedmioty" className={navLinkClassName}>
              Przedmioty
            </Link>

            <div className="flex items-center gap-4 justify-end pl-2 min-w-[40px]">
              <ThemeToggle />
              <AuthAction variant="desktop" />
            </div>
          </div>
        </nav>
      </div>

      <div className="w-full h-16 px-8 flex items-center justify-between md:hidden">
        <Logo size="mobile" />

        <button
          type="button"
          onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
          className="text-muted-foreground hover:text-foreground focus:outline-none p-2"
        >
          {isMobileMenuOpen ? <X className="h-6 w-6" /> : <Menu className="h-6 w-6" />}
        </button>
      </div>

      <MobileMenu open={isMobileMenuOpen} onClose={() => setIsMobileMenuOpen(false)} />
    </header>
  );
}
