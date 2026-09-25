import '@/styles/tailwind.css';
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { App } from '@/App';
import { isMockingEnabled } from '@/lib/mocks';

async function enableMocking() {
  if (!isMockingEnabled) return;

  const { worker } = await import('./tests/__mocks__/browser');
  return worker.start();
}

enableMocking().then(() =>
  createRoot(document.getElementById('root')!).render(
    <StrictMode>
      <App />
    </StrictMode>
  )
);
