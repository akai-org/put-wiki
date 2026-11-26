import { ChevronDown, Sun } from 'lucide-react';
import { useTheme } from '@/hooks/useTheme';
import type { Theme } from '@/contexts/ThemeContext';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuLabel,
  DropdownMenuRadioGroup,
  DropdownMenuRadioItem,
  DropdownMenuTrigger,
} from '@/components/ui/DropdownMenu';
import { cn } from '@/utils';
import { focusRing } from '../focusRing';

const themeOptions: { value: Theme; label: string }[] = [
  { value: 'light', label: 'Jasny' },
  { value: 'dark', label: 'Ciemny' },
];

function isTheme(value: string): value is Theme {
  return themeOptions.some((option) => option.value === value);
}

export function AppearanceMenu({ className }: { className?: string }) {
  const { theme, setTheme } = useTheme();

  return (
    <DropdownMenu>
      <DropdownMenuTrigger
        className={cn(
          'group flex h-10 shrink-0 items-center gap-1.5 rounded-sm px-1 text-sm font-normal whitespace-nowrap text-foreground transition-opacity hover:opacity-85',
          focusRing,
          className
        )}
      >
        <Sun aria-hidden="true" className="size-4.5" />
        Wygląd
        <ChevronDown
          aria-hidden="true"
          className="size-4 transition-transform group-data-[state=open]:rotate-180"
        />
      </DropdownMenuTrigger>
      <DropdownMenuContent align="end">
        <DropdownMenuLabel>Motyw</DropdownMenuLabel>
        <DropdownMenuRadioGroup
          onValueChange={(value) => isTheme(value) && setTheme(value)}
          value={theme}
        >
          {themeOptions.map(({ value, label }) => (
            <DropdownMenuRadioItem key={value} value={value}>
              {label}
            </DropdownMenuRadioItem>
          ))}
        </DropdownMenuRadioGroup>
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
