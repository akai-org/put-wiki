import { cn } from '@/utils';
import { AppearanceMenu } from '@/components/layout/header/components/AppearanceMenu';
import { AuthAction } from '@/components/layout/header/components/AuthAction';
import { Logo } from '@/components/layout/header/components/Logo';
import { MobileMenu } from '@/components/layout/header/components/MobileMenu';
import { NavLinks } from '@/components/layout/header/components/NavLinks';
import { SearchBar } from '@/components/layout/header/components/SearchBar';

export type HeaderVariant = 'solid' | 'transparent';

export interface HeaderProps {
  variant?: HeaderVariant;
  showAuthAction?: boolean;
}

const variantClassName: Record<HeaderVariant, string> = {
  solid: 'bg-background shadow-header',
  transparent: '-mb-16 bg-brand/40 backdrop-blur-sm lg:-mb-22',
};

export default function Header({ variant = 'solid', showAuthAction = true }: HeaderProps) {
  return (
    <header
      aria-label="PUTWiki"
      className={cn(
        'sticky top-0 z-50 w-full text-foreground select-none',
        variantClassName[variant]
      )}
    >
      <div className="mx-auto flex h-16 w-full max-w-header items-center gap-5.5 px-5 sm:px-8 lg:h-22">
        <Logo />
        <NavLinks className="hidden lg:block" />

        <div className="hidden flex-1 items-center justify-end gap-5.5 lg:flex">
          <SearchBar className="max-w-search" />
          <AppearanceMenu />
          {showAuthAction ? <AuthAction variant="desktop" /> : null}
        </div>

        <MobileMenu className="ml-auto lg:hidden" showAuthAction={showAuthAction} />
      </div>
    </header>
  );
}
