import { User } from 'lucide-react';
import { Link } from '@tanstack/react-router';
import { useAuth } from '@/hooks/useAuth';
import { useUser } from '@/hooks/useUser';

export function AuthAction({
  variant,
  onAction,
}: {
  variant: 'desktop' | 'mobile';
  onAction?: () => void;
}) {
  const { isLoggedIn, login } = useAuth();
  const { nickname } = useUser();

  if (variant === 'mobile') {
    return isLoggedIn ? (
      <div className="flex items-center gap-3">
        <div className="h-9 w-9 rounded-full border border-border flex items-center justify-center text-muted-foreground bg-muted">
          <User className="h-5 w-5" />
        </div>
        <span className="text-md text-muted-foreground">Witaj, {nickname}</span>
      </div>
    ) : (
      <button
        type="button"
        onClick={() => {
          login();
          onAction?.();
        }}
        className="w-full border border-border hover:border-foreground text-foreground font-medium py-2 rounded-full text-center flex items-center justify-center gap-2"
      >
        <User className="h-4 w-4" /> Zaloguj
      </button>
    );
  }

  return isLoggedIn ? (
    <Link to="/profile" className="flex items-center gap-3 hover:opacity-90 transition-all group">
      <span className="text-sm font-medium font-serif text-muted-foreground group-hover:text-foreground whitespace-nowrap">
        Witaj, {nickname}
      </span>
      <div className="h-10 w-10 rounded-full border border-border flex items-center justify-center text-muted-foreground bg-muted group-hover:border-foreground transition-colors flex-shrink-0">
        <User className="h-5 w-5" />
      </div>
    </Link>
  ) : (
    <button
      type="button"
      onClick={login}
      className="text-muted-foreground hover:text-foreground transition-colors focus:outline-none flex-shrink-0"
      title="Zaloguj się"
    >
      <div className="h-10 w-10 rounded-full border border-border flex items-center justify-center bg-transparent hover:border-foreground transition-colors">
        <User className="h-6 w-6" />
      </div>
    </button>
  );
}
