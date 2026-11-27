import { act, screen, waitFor, within } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { afterEach, beforeEach, describe, expect, it } from 'vitest';
import { server } from '@/tests/__mocks__/node';
import { authHandlers } from '@/tests/__mocks__/authHandlers';
import { renderWithProviders } from '@/tests/utils/renderWithProviders';
import type { AppProvidersOptions } from '@/tests/utils/AppProviders';
import { Header, type HeaderProps } from '..';

async function renderHeader(props: HeaderProps = {}, options: AppProvidersOptions = {}) {
  renderWithProviders(<Header {...props} />, { theme: 'light', ...options });
  return screen.findByRole('banner');
}

function getMainNav() {
  return screen.getByRole('navigation', { name: 'Główna nawigacja' });
}

describe('Header', () => {
  beforeEach(() => {
    server.use(...authHandlers.loggedOut);
  });

  afterEach(() => {
    localStorage.clear();
    document.documentElement.classList.remove('dark');
  });

  describe('links', () => {
    it('renders the logo as a link to the home page', async () => {
      await renderHeader();

      expect(screen.getByRole('link', { name: 'PUTWiki – strona główna' })).toHaveAttribute(
        'href',
        '/'
      );
    });

    it('renders catalog links in the main navigation', async () => {
      await renderHeader();

      const nav = getMainNav();
      expect(within(nav).getByRole('link', { name: 'Przedmioty' })).toHaveAttribute(
        'href',
        '/courses'
      );
      expect(within(nav).getByRole('link', { name: 'Prowadzący' })).toHaveAttribute(
        'href',
        '/lecturers'
      );
    });

    it('marks the link of the current section as the current page', async () => {
      await renderHeader({}, { initialPath: '/courses/algorytmy' });

      const nav = getMainNav();
      expect(within(nav).getByRole('link', { name: 'Przedmioty' })).toHaveAttribute(
        'aria-current',
        'page'
      );
      expect(within(nav).getByRole('link', { name: 'Prowadzący' })).not.toHaveAttribute(
        'aria-current'
      );
    });

    it('does not mark any section link on the welcome page', async () => {
      await renderHeader({ variant: 'transparent' }, { initialPath: '/' });

      const nav = getMainNav();
      for (const name of ['Przedmioty', 'Prowadzący']) {
        expect(within(nav).getByRole('link', { name })).not.toHaveAttribute('aria-current');
      }
    });
  });

  describe('search', () => {
    it('renders a labelled search field with the catalog placeholder', async () => {
      await renderHeader();

      const search = within(screen.getByRole('search')).getByRole('searchbox', {
        name: 'Szukaj przedmiotów i prowadzących',
      });
      expect(search).toHaveAttribute('placeholder', 'Szukaj przedmiotów i prowadzących');
    });
  });

  describe('"Wygląd" control', () => {
    it('shows the current theme as the selected option', async () => {
      const user = userEvent.setup();
      await renderHeader();

      await user.click(screen.getByRole('button', { name: 'Wygląd' }));

      expect(await screen.findByRole('menuitemradio', { name: 'Jasny' })).toHaveAttribute(
        'aria-checked',
        'true'
      );
      expect(screen.getByRole('menuitemradio', { name: 'Ciemny' })).toHaveAttribute(
        'aria-checked',
        'false'
      );
    });

    it('switches to the dark theme when "Ciemny" is chosen', async () => {
      const user = userEvent.setup();
      await renderHeader();

      await user.click(screen.getByRole('button', { name: 'Wygląd' }));
      await user.click(await screen.findByRole('menuitemradio', { name: 'Ciemny' }));

      expect(document.documentElement).toHaveClass('dark');
      expect(localStorage.getItem('theme')).toBe('dark');
    });

    it('can be operated with the keyboard', async () => {
      const user = userEvent.setup();
      await renderHeader();
      const trigger = screen.getByRole('button', { name: 'Wygląd' });

      act(() => trigger.focus());
      await user.keyboard('{Enter}');
      expect(await screen.findByRole('menuitemradio', { name: 'Jasny' })).toHaveFocus();

      await user.keyboard('{ArrowDown}');
      expect(screen.getByRole('menuitemradio', { name: 'Ciemny' })).toHaveFocus();

      await user.keyboard('{Enter}');
      await waitFor(() => expect(screen.queryByRole('menu')).not.toBeInTheDocument());
      expect(document.documentElement).toHaveClass('dark');
      expect(trigger).toHaveFocus();
    });
  });

  describe('auth action', () => {
    it('shows the login button by default', async () => {
      await renderHeader();

      expect(screen.getByRole('button', { name: /Zaloguj z USOS/ })).toBeInTheDocument();
    });

    it('greets the logged-in user', async () => {
      server.use(...authHandlers.loggedIn);
      await renderHeader();

      expect(await screen.findByText('Witaj, Lolek')).toBeInTheDocument();
    });

    it('hides the auth action when showAuthAction is false', async () => {
      await renderHeader({ variant: 'transparent', showAuthAction: false });

      expect(screen.queryByRole('button', { name: /Zaloguj z USOS/ })).not.toBeInTheDocument();
    });
  });

  describe('mobile menu', () => {
    it('opens a dialog with navigation, search and the "Wygląd" control', async () => {
      const user = userEvent.setup();
      await renderHeader();

      const toggle = screen.getByRole('button', { name: 'Otwórz menu' });
      expect(toggle).toHaveAttribute('aria-expanded', 'false');

      await user.click(toggle);

      const dialog = await screen.findByRole('dialog', { name: 'Menu nawigacyjne' });
      expect(toggle).toHaveAttribute('aria-expanded', 'true');
      expect(within(dialog).getByRole('link', { name: 'Przedmioty' })).toBeInTheDocument();
      expect(within(dialog).getByRole('link', { name: 'Prowadzący' })).toBeInTheDocument();
      expect(within(dialog).getByRole('searchbox')).toBeInTheDocument();
      expect(within(dialog).getByRole('button', { name: 'Wygląd' })).toBeInTheDocument();
    });

    it('closes after choosing a link', async () => {
      const user = userEvent.setup();
      await renderHeader();

      await user.click(screen.getByRole('button', { name: 'Otwórz menu' }));
      const dialog = await screen.findByRole('dialog', { name: 'Menu nawigacyjne' });
      await user.click(within(dialog).getByRole('link', { name: 'Prowadzący' }));

      await waitFor(() => expect(screen.queryByRole('dialog')).not.toBeInTheDocument());
    });

    it('closes with its close button', async () => {
      const user = userEvent.setup();
      await renderHeader();

      await user.click(screen.getByRole('button', { name: 'Otwórz menu' }));
      const dialog = await screen.findByRole('dialog', { name: 'Menu nawigacyjne' });
      await user.click(within(dialog).getByRole('button', { name: 'Zamknij menu' }));

      await waitFor(() => expect(screen.queryByRole('dialog')).not.toBeInTheDocument());
    });
  });
});
