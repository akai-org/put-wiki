import type { Decorator, Meta, StoryObj } from '@storybook/react-vite';
import { expect, waitFor, within } from 'storybook/test';
import Header from './Header';
import { withAppProviders } from '@/tests/utils/withAppProviders';
import { authHandlers } from '@/tests/__mocks__/authHandlers';

// Stand-in for the welcome page gradient (owned by the welcome page task). `primary` has
// about the same lightness as the lightest Figma stop (#A297BC), so contrast checks stay honest.
const withWelcomeGradient: Decorator = (Story) => (
  <div className="min-h-screen bg-linear-to-br from-primary via-muted to-background">
    <Story />
    <section className="px-8 pt-40">
      <p className="text-4xl font-bold text-foreground">Strona powitalna</p>
    </section>
  </div>
);

const meta = {
  title: 'Layout/Header',
  component: Header,
  parameters: {
    layout: 'fullscreen',
    theme: 'light',
    msw: authHandlers.loggedOut,
  },
  args: {
    variant: 'solid',
    showAuthAction: true,
  },
  argTypes: {
    variant: { control: 'inline-radio', options: ['solid', 'transparent'] },
  },
  decorators: [withAppProviders],
} satisfies Meta<typeof Header>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Standard: Story = {};

export const StandardLoggedIn: Story = {
  parameters: { msw: authHandlers.loggedIn },
};

export const StandardActiveLink: Story = {
  parameters: { router: { initialPath: '/courses' } },
};

export const StandardDark: Story = {
  parameters: { theme: 'dark' },
};

export const Transparent: Story = {
  args: { variant: 'transparent', showAuthAction: false },
  decorators: [withWelcomeGradient],
};

export const TransparentDark: Story = {
  ...Transparent,
  parameters: { theme: 'dark' },
};

const openMobileMenu: Story['play'] = async ({ canvas, canvasElement, userEvent }) => {
  await userEvent.click(await canvas.findByRole('button', { name: 'Otwórz menu' }));

  const dialog = await within(canvasElement.ownerDocument.body).findByRole('dialog', {
    name: 'Menu nawigacyjne',
  });
  // The sheet fades in from opacity 0, so wait for the animation instead of checking the first frame.
  await waitFor(() =>
    expect(within(dialog).getByRole('navigation', { name: 'Główna nawigacja' })).toBeVisible()
  );
};

export const Mobile: Story = {
  globals: { viewport: { value: 'mobile2', isRotated: false } },
  play: openMobileMenu,
};

export const MobileLoggedIn: Story = {
  ...Mobile,
  parameters: { msw: authHandlers.loggedIn },
};
