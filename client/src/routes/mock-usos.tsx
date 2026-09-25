import { createFileRoute, notFound } from '@tanstack/react-router';
import { useState } from 'react';
import { z } from 'zod';
import { Button } from '@/components/ui/Button';
import { toSafeRedirectPath } from '@/features/auth';
import { api } from '@/lib/api';
import { isMockingEnabled } from '@/lib/mocks';

const mockUsosSearchSchema = z.object({
  oauth_token: z.string(),
  returnUrl: z.string().optional(),
});

// Fake USOS authorization page, available only with MSW mocks enabled.
// It replaces the part of the login flow which happens outside our app and can't be intercepted by MSW.
export const Route = createFileRoute('/mock-usos')({
  validateSearch: mockUsosSearchSchema,
  beforeLoad: () => {
    if (!isMockingEnabled) {
      throw notFound();
    }
  },
  component: MockUsosPage,
});

function MockUsosPage() {
  const { oauth_token: oauthToken, returnUrl } = Route.useSearch();
  const [isAuthorizing, setIsAuthorizing] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const redirectPath = toSafeRedirectPath(returnUrl);

  async function authorize() {
    setIsAuthorizing(true);
    setError(null);

    try {
      // In the real flow USOS redirects the browser to the callback with oauth_verifier
      await api.get('/api/auth/callback', {
        params: { oauth_token: oauthToken, oauth_verifier: crypto.randomUUID() },
      });
      window.location.assign(redirectPath);
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Nieznany błąd');
      setIsAuthorizing(false);
    }
  }

  return (
    <div className="flex w-full max-w-lg flex-col gap-6 px-8 py-16 font-serif">
      <p className="rounded-md border border-dashed border-border px-3 py-2 text-center text-sm text-muted-foreground">
        Mock stronka USOSa, widoczna tylko jak MSW jest włączone.
      </p>
      <h1 className="text-2xl font-bold">Autoryzacja aplikacji</h1>
      <p className="text-muted-foreground">
        <strong>PutWiki</strong> prosi o dostęp do Twojego konta USOS.
      </p>
      <p className="text-sm break-all text-muted-foreground">oauth_token: {oauthToken}</p>
      {error ? <p className="text-sm text-destructive">Błąd callbacku: {error}</p> : null}
      <div className="flex gap-3">
        <Button className="bg-green-600" disabled={isAuthorizing} onClick={authorize} type="button">
          Zezwól
        </Button>
        <Button
          className="bg-red-600"
          disabled={isAuthorizing}
          onClick={() => window.location.assign(redirectPath)}
          type="button"
        >
          Odmów
        </Button>
      </div>
    </div>
  );
}
