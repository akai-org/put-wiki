import { useEffect, useState, type ReactNode } from 'react';
import { ThemeContext } from './ThemeContext';

const STORAGE_KEY = 'theme';

function getStoredTheme(): boolean | null {
  const stored = localStorage.getItem(STORAGE_KEY);
  if (stored === 'dark') return true;
  if (stored === 'light') return false;
  return null;
}

function getPreferredTheme(): boolean {
  return window.matchMedia('(prefers-color-scheme: dark)').matches;
}

export function ThemeProvider({
  children,
  initialDark,
}: {
  children: ReactNode;
  initialDark?: boolean;
}) {
  const [isDark, setIsDark] = useState(
    () => initialDark ?? getStoredTheme() ?? getPreferredTheme()
  );

  useEffect(() => {
    document.documentElement.classList.toggle('dark', isDark);
    localStorage.setItem(STORAGE_KEY, isDark ? 'dark' : 'light');
  }, [isDark]);

  return <ThemeContext.Provider value={{ isDark, setIsDark }}>{children}</ThemeContext.Provider>;
}
