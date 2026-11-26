import { User } from 'lucide-react';
import { Link } from '@tanstack/react-router';
import { useUserSession } from '@/features/auth';
import { cn } from '@/utils';
import { focusRing } from '../focusRing';

export function AuthAction({
  variant,
  onAction,
}: {
  variant: 'desktop' | 'mobile';
  onAction?: () => void;
}) {
  const { user, isLoggedIn, login } = useUserSession();

  if (variant === 'mobile') {
    return isLoggedIn ? (
      <div className="flex items-center gap-3">
        <div className="flex size-9 items-center justify-center rounded-full border border-border bg-muted text-muted-foreground">
          <User aria-hidden="true" className="size-5" />
        </div>
        <span className="text-muted-foreground">Witaj, {user?.nickname}</span>
      </div>
    ) : (
      <button
        className={cn(
          'flex w-full items-center justify-center gap-2 rounded-full border border-border py-2 text-center font-medium text-foreground hover:border-foreground',
          focusRing
        )}
        onClick={() => {
          login();
          onAction?.();
        }}
        type="button"
      >
        <User aria-hidden="true" className="size-4" /> Zaloguj z USOS
      </button>
    );
  }

  return isLoggedIn ? (
    <Link
      className={cn(
        'group flex items-center gap-3 rounded-full transition-all hover:opacity-90',
        focusRing
      )}
      to="/"
    >
      <span className="text-sm font-medium whitespace-nowrap text-muted-foreground group-hover:text-foreground">
        Witaj, {user?.nickname}
      </span>
      <div className="flex size-10 shrink-0 items-center justify-center rounded-full border border-border bg-muted text-muted-foreground transition-colors group-hover:border-foreground">
        <User aria-hidden="true" className="size-5" />
      </div>
    </Link>
  ) : (
    <button
      className={cn(
        'flex h-10 shrink-0 items-center gap-2 rounded-full border border-border px-4 text-sm font-medium whitespace-nowrap text-foreground transition-colors hover:border-foreground',
        focusRing
      )}
      onClick={login}
      type="button"
    >
      <User aria-hidden="true" className="size-4" />
      Zaloguj z USOS
    </button>
  );
}
