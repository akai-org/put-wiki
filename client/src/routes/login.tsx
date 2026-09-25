import { createFileRoute, redirect } from '@tanstack/react-router';
import { User } from 'lucide-react';
import { z } from 'zod';
import { Button } from '@/components/ui/Button';
import { queries, toSafeRedirectPath, useUserSession } from '@/features/auth';

const loginSearchSchema = z.object({
  redirect: z.string().optional(),
});

export const Route = createFileRoute('/login')({
  validateSearch: loginSearchSchema,
  beforeLoad: async ({ context: { queryClient }, search }) => {
    const user = await queryClient.ensureQueryData(queries.user());

    if (user) {
      throw redirect({ href: toSafeRedirectPath(search.redirect) });
    }
  },
  component: LoginPage,
});

function LoginPage() {
  const { redirect: redirectPath } = Route.useSearch();
  const { login, isLoggingIn } = useUserSession();

  return (
    <div className="flex flex-col items-center gap-6 px-8 py-24 text-center font-serif">
      <h1 className="text-3xl font-bold">Zaloguj się</h1>
      <p className="max-w-md text-muted-foreground">
        Ta strona wymaga zalogowania. Użyj swojego konta USOS.
      </p>
      <Button
        disabled={isLoggingIn}
        onClick={() => login(toSafeRedirectPath(redirectPath))}
        size="lg"
        type="button"
        variant="outline"
      >
        <User /> Zaloguj z USOS
      </Button>
    </div>
  );
}
