import { useEffect, useState } from 'react';
import { Menu, X, Settings, User } from 'lucide-react';
import { Link } from '@tanstack/react-router';

export default function Header({ 
  defaultLoggedIn = false, 
  defaultDark = false 
}: { 
  defaultLoggedIn?: boolean; 
  defaultDark?: boolean;
}) {
  const [isLoggedIn, setIsLoggedIn] = useState(defaultLoggedIn);
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);
  const [isDark, setIsDark] = useState(defaultDark);
  const mockNickname = 'Janek';

  useEffect(() => {
    setIsLoggedIn(defaultLoggedIn);
  }, [defaultLoggedIn]);

  useEffect(() => {
    setIsDark(defaultDark);
  }, [defaultDark]);

  // Zarządzanie klasą 'dark' w dokumencie
  useEffect(() => {
    if (isDark) {
      document.documentElement.classList.add('dark');
    } else {
      document.documentElement.classList.remove('dark');
    }
  }, [isDark]);

  return (
    <header className="sticky top-0 z-50 w-full bg-background text-foreground border-b border-border shadow-md select-none">
      <div className="w-full flex justify-center px-8 md:px-16">
        <div className="w-full max-w-[1200px] h-20 hidden md:grid grid-cols-3 items-center">
          
          <div className="flex items-center gap-10 justify-start">
            <Link
              to="/"
              className="text-3xl font-black font-serif tracking-tight text-foreground hover:opacity-85 transition-opacity"
            >
              PUTWiki
            </Link>
            <nav>
              <Link
                to="/prowadzacy"
                className="text-[15px] font-normal font-serif tracking-wider text-muted-foreground hover:text-foreground transition-colors uppercase"
              >
                Prowadzący
              </Link>
            </nav>
          </div>

          <div className="flex justify-center w-full">
            <div className="relative flex items-center gap-3.5 bg-muted rounded-full pl-2.5 pr-4 h-12 w-full max-w-[340px] border border-input focus-within:border-ring transition-all">
              <div className="bg-background w-10 h-10 rounded-full text-muted-foreground flex items-center justify-center shadow-inner flex-shrink-0">
                <Settings className="h-5 w-5" />
              </div>
              <input
                type="text"
                placeholder="Wyszukaj"
                className="w-full bg-transparent text-[15px] text-foreground font-serif placeholder:text-muted-foreground focus:outline-none"
              />
            </div>
          </div>

          <div className="flex items-center gap-10 justify-end">
            <Link
              to="/przedmioty"
              className="text-[15px] font-normal font-serif tracking-wider text-muted-foreground hover:text-foreground transition-colors uppercase"
            >
              Przedmioty
            </Link>

            <div className="flex items-center justify-end min-w-[100px]">
              {isLoggedIn ? (
                <Link
                  to="/profile"
                  className="flex items-center gap-3 hover:opacity-90 transition-all group"
                >
                  <span className="text-sm font-medium font-serif text-muted-foreground group-hover:text-foreground whitespace-nowrap">
                    Witaj, {mockNickname}
                  </span>
                  <div className="h-10 w-10 rounded-full border border-border flex items-center justify-center text-muted-foreground bg-muted group-hover:border-foreground transition-colors flex-shrink-0">
                    <User className="h-5 w-5" />
                  </div>
                </Link>
              ) : (
                <button
                  type="button"
                  onClick={() => setIsLoggedIn(true)}
                  className="text-muted-foreground hover:text-foreground transition-colors focus:outline-none flex-shrink-0"
                  title="Zaloguj się"
                >
                  <div className="h-10 w-10 rounded-full border border-border flex items-center justify-center bg-transparent hover:border-foreground transition-colors">
                    <User className="h-6 w-6" />
                  </div>
                </button>
              )}
            </div>
          </div>

        </div>
      </div>

      <div className="w-full h-16 px-6 flex items-center justify-between md:hidden">
        <Link to="/" className="text-2xl font-black font-serif tracking-tight text-foreground">
          PUTWiki
        </Link>

        <button
          type="button"
          onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
          className="text-muted-foreground hover:text-foreground focus:outline-none p-2"
        >
          {isMobileMenuOpen ? <X className="h-6 w-6" /> : <Menu className="h-6 w-6" />}
        </button>
      </div>

      {isMobileMenuOpen && (
        <div className="md:hidden bg-background border-t border-border px-6 py-6 flex flex-col gap-4 absolute left-0 w-full shadow-xl z-50">
          <div className="relative flex items-center gap-2 bg-muted rounded-full pl-2 pr-4 h-11 w-full">
            <div className="bg-background w-10 h-10 rounded-full text-muted-foreground flex items-center justify-center flex-shrink-0">
              <Settings className="h-5 w-5" />
            </div>
            <input
              type="text"
              placeholder="Wyszukaj..."
              className="w-full bg-transparent text-sm text-foreground placeholder:text-muted-foreground focus:outline-none"
            />
          </div>

          <nav className="flex flex-col gap-4 font-serif">
            <Link
              to="/prowadzacy"
              className="text-lg text-muted-foreground hover:text-foreground uppercase tracking-wider"
            >
              Prowadzący
            </Link>
            <Link
              to="/przedmioty"
              className="text-lg text-muted-foreground hover:text-foreground uppercase tracking-wider"
            >
              Przedmioty
            </Link>
          </nav>

          <div className="border-t border-border pt-4 font-serif">
            {isLoggedIn ? (
              <div className="flex items-center gap-3">
                <div className="h-9 w-9 rounded-full border border-border flex items-center justify-center text-muted-foreground bg-muted">
                  <User className="h-5 w-5" />
                </div>
                <span className="text-md text-muted-foreground">Witaj, {mockNickname}</span>
              </div>
            ) : (
              <button
                type="button"
                onClick={() => setIsLoggedIn(true)}
                className="w-full border border-border hover:border-foreground text-foreground font-medium py-2 rounded-full text-center flex items-center justify-center gap-2"
              >
                <User className="h-4 w-4" /> Zaloguj
              </button>
            )}
          </div>
        </div>
      )}
    </header>
  );
}