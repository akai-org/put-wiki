import { Moon, Sun } from 'lucide-react';
import { useTheme } from '@/hooks/useTheme';

export function ThemeToggle() {
  const { isDark, setTheme } = useTheme();

  return (
    <button
      className="text-muted-foreground hover:text-foreground transition-colors focus:outline-none flex-shrink-0"
      onClick={() => setTheme(isDark ? 'light' : 'dark')}
      title={isDark ? 'Przełącz na jasny motyw' : 'Przełącz na ciemny motyw'}
      type="button"
    >
      {isDark ? <Sun className="h-5 w-5" /> : <Moon className="h-5 w-5" />}
    </button>
  );
}
