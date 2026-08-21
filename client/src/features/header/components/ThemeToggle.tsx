import { Moon, Sun } from 'lucide-react';
import { useTheme } from '@/hooks/useTheme';

export function ThemeToggle() {
  const { isDark, setTheme } = useTheme();

  return (
    <button
      className="flex-shrink-0 text-muted-foreground transition-colors hover:text-foreground focus:outline-none"
      onClick={() => setTheme(isDark ? 'light' : 'dark')}
      title={isDark ? 'Przełącz na jasny motyw' : 'Przełącz na ciemny motyw'}
      type="button"
    >
      {isDark ? <Sun className="size-5" /> : <Moon className="size-5" />}
    </button>
  );
}
