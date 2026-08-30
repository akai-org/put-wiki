import { type ErrorComponentProps } from '@tanstack/react-router';
import { useEffect } from 'react';

export default function GlobalErrorPage({ error, reset, info }: ErrorComponentProps) {
  useEffect(() => {
    console.error(`Stack trace: ${info}`);
  }, [info]);

  return (
    <div>
      <h1>Ups! Coś poszło nie tak.</h1>
      <p>Wystąpił niespodziewany błąd:</p>

      <div>
        <code>{error.message}</code>
      </div>

      <button onClick={() => reset()} type="button">
        Spróbuj ponownie
      </button>
    </div>
  );
}
