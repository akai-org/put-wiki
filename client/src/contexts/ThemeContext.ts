import { createContext } from 'react';

export interface ThemeContextValue {
  isDark: boolean;
  setIsDark: (isDark: boolean) => void;
}

export const ThemeContext = createContext<ThemeContextValue | null>(null);
