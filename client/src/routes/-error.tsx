import { type ErrorComponentProps, useNavigate } from '@tanstack/react-router';
import axios from 'axios';
import { useEffect } from 'react';
import GlobalNotFoundPage from './-not-found';

export default function GlobalErrorPage({ error, info }: ErrorComponentProps) {
  useEffect(() => {
    console.error(`Stack trace: ${info}`);
  }, [info]);
  const navigate = useNavigate();

  if (axios.isAxiosError(error) && error.response?.status === 404) {
    return <GlobalNotFoundPage />;
  }

  return (
    <div className="flex size-full flex-col items-center justify-center p-6">
      <h1 className="text-6xl font-bold text-destructive">
        {'status' in error ? String(error.status) : '500'}
      </h1>
      <div>
        <code>{error.message}</code>
      </div>

      <button
        className="my-2 rounded-lg bg-secondary px-2 py-1 text-xl text-secondary-foreground transition-colors hover:bg-secondary/80 focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 focus-visible:outline-none"
        onClick={() => navigate({ to: '/' })}
        type="button"
      >
        Go back to home
      </button>
    </div>
  );
}
