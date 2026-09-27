import { Button } from '@/components/ui/Button';
import { type ErrorComponentProps, useNavigate } from '@tanstack/react-router';
import { useEffect } from 'react';

export default function GlobalErrorPage({ error, info }: ErrorComponentProps) {
  useEffect(() => {
    console.error(`Stack trace: ${info}`);
  }, [info]);
  const navigate = useNavigate();

  return (
    <div className="flex size-full flex-col items-center justify-center p-6">
      <h1 className="text-6xl font-bold text-destructive">
        {'status' in error ? String(error.status) : '500'}
      </h1>
      <div>
        <code>{error.message}</code>
      </div>

      <Button  variant="secondary" onClick={() => navigate({ to: '/' })}>
        Go back to home
      </Button>
    </div>
  );
}
