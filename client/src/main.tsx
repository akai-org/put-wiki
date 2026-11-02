import '@/styles/tailwind.css';
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { App } from '@/App';

async function enableMocking() {
  if (!import.meta.env.DEV) return;

  // eslint-disable-next-line import/no-unresolved
  const { network } = await import('virtual:msw');
  const { handlers } = await import('./tests/__mocks__/handlers');

  network.configure({ handlers });
  await network.enable();
}

enableMocking().then(() =>
  createRoot(document.getElementById('root')!).render(
    <StrictMode>
      <App />
    </StrictMode>
  )
);
