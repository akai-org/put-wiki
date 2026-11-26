import type { FormEvent } from 'react';
import { Search } from 'lucide-react';
import { cn } from '@/utils';

const SEARCH_LABEL = 'Szukaj przedmiotów i prowadzących';

// Search logic is delivered by the Search engine epic (#25); for now the form only renders.
function handleSubmit(event: FormEvent<HTMLFormElement>) {
  event.preventDefault();
}

export function SearchBar({ className }: { className?: string }) {
  return (
    <form
      className={cn(
        'flex h-10.5 w-full items-center gap-2.5 rounded-search border border-foreground/18 bg-brand/65 px-3.5 text-foreground transition-colors',
        'focus-within:outline-2 focus-within:outline-offset-2 focus-within:outline-ring',
        className
      )}
      onSubmit={handleSubmit}
      role="search"
    >
      <Search aria-hidden="true" className="size-4.5 shrink-0 text-search-icon" />
      <input
        aria-label={SEARCH_LABEL}
        className="w-full min-w-0 bg-transparent text-base text-foreground placeholder:text-search-placeholder focus:outline-none"
        placeholder={SEARCH_LABEL}
        type="search"
      />
    </form>
  );
}
