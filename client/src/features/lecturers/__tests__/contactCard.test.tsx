import { cleanup, render, screen } from '@testing-library/react';
import { afterEach, describe, expect, it } from 'vitest';

import ContactCard from '../components/ContactCard';

const contactInfo = {
  email: 'jan.kowalski@example.com',
  phone: '+48 600 123 456',
  websiteUrl: 'https://example.com/jan-kowalski',
};

afterEach(() => {
  cleanup();
});

describe('ContactCard', () => {
  it('renders the contact section heading', () => {
    render(<ContactCard {...contactInfo} />);

    expect(screen.getByText('Kontakt')).toBeTruthy();
  });

  it('renders email, phone and website links with the correct href values', () => {
    render(<ContactCard {...contactInfo} />);

    expect(screen.getByRole('link', { name: contactInfo.email }).getAttribute('href')).toBe(
      `mailto:${contactInfo.email}`
    );
    expect(screen.getByRole('link', { name: contactInfo.phone }).getAttribute('href')).toBe(
      `tel:${contactInfo.phone}`
    );
    expect(screen.getByRole('link', { name: contactInfo.websiteUrl }).getAttribute('href')).toBe(
      contactInfo.websiteUrl
    );
  });
});
