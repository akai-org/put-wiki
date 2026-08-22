import '@/styles/tailwind.css';
import '@/styles/global.css';
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { App } from '@/App';

async function enableMocking() {
  if (process.env.NODE_ENV !== 'development') return;

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
