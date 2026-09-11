import { LogOut, User } from 'lucide-react';
import { Link } from '@tanstack/react-router';
import { useUserSession } from '@/features/auth';

export function AuthAction({
  variant,
  onAction,
}: {
  variant: 'desktop' | 'mobile';
  onAction?: () => void;
}) {
  const { isLoggedIn, login, logout, isLoggingOut } = useUserSession();

  if (variant === 'mobile') {
    return isLoggedIn ? (
      <div className="flex items-center justify-between gap-3">
        <Link className="flex items-center gap-3" onClick={onAction} to="/profile">
          <div className="flex size-9 items-center justify-center rounded-full border border-border bg-muted text-muted-foreground">
            <User className="size-5" />
          </div>
          <span className="text-muted-foreground">Mój profil</span>
        </Link>
        <button
          className="flex items-center gap-2 rounded-full border border-border px-3 py-2 font-medium text-foreground hover:border-foreground disabled:opacity-50"
          disabled={isLoggingOut}
          onClick={() => {
            logout();
            onAction?.();
          }}
          type="button"
        >
          <LogOut className="size-4" /> Wyloguj
        </button>
      </div>
    ) : (
      <button
        className="flex w-full items-center justify-center gap-2 rounded-full border border-border py-2 text-center font-medium text-foreground hover:border-foreground"
        onClick={() => {
          login();
          onAction?.();
        }}
        type="button"
      >
        <User className="size-4" /> Zaloguj z USOS
      </button>
    );
  }

  return isLoggedIn ? (
    <div className="flex items-center gap-3">
      <Link className="group flex items-center gap-3 transition-all hover:opacity-90" to="/profile">
        <span className="font-serif text-sm font-medium whitespace-nowrap text-muted-foreground group-hover:text-foreground">
          Mój profil
        </span>
        <div className="flex size-10 shrink-0 items-center justify-center rounded-full border border-border bg-muted text-muted-foreground transition-colors group-hover:border-foreground">
          <User className="size-5" />
        </div>
      </Link>
      <button
        aria-label="Wyloguj"
        className="flex size-10 shrink-0 items-center justify-center rounded-full border border-border text-muted-foreground transition-colors hover:border-foreground hover:text-foreground disabled:opacity-50"
        disabled={isLoggingOut}
        onClick={logout}
        title="Wyloguj"
        type="button"
      >
        <LogOut className="size-5" />
      </button>
    </div>
  ) : (
    <button
      className="flex h-10 shrink-0 items-center gap-2 rounded-full border border-border px-4 font-serif text-sm font-medium whitespace-nowrap text-foreground transition-colors hover:border-foreground focus:outline-none"
      onClick={() => login()}
      type="button"
    >
      <User className="size-4" />
      Zaloguj z USOS
    </button>
  );
}
