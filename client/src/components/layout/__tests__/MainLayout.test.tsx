import { screen, within } from '@testing-library/react';
import { beforeEach, describe, expect, it } from 'vitest';
import { server } from '@/tests/__mocks__/node';
import { authHandlers } from '@/tests/__mocks__/authHandlers';
import { renderWithProviders } from '@/tests/utils/renderWithProviders';
import { MainLayout } from '../MainLayout';

async function renderLayoutAt(initialPath: string) {
  renderWithProviders(
    <MainLayout>
      <p>Treść strony</p>
    </MainLayout>,
    { initialPath, theme: 'light' }
  );
  return screen.findByRole('banner');
}

describe('MainLayout', () => {
  beforeEach(() => {
    server.use(...authHandlers.loggedOut);
  });

  it('renders the page content', async () => {
    await renderLayoutAt('/courses');

    expect(screen.getByText('Treść strony')).toBeInTheDocument();
  });

  it('leaves the login action to the hero on the welcome page', async () => {
    const header = await renderLayoutAt('/');

    expect(
      within(header).queryByRole('button', { name: /Zaloguj z USOS/ })
    ).not.toBeInTheDocument();
  });

  it('shows the login action in the header on other pages', async () => {
    const header = await renderLayoutAt('/lecturers/jan-kowalski');

    expect(within(header).getByRole('button', { name: /Zaloguj z USOS/ })).toBeInTheDocument();
  });
});
