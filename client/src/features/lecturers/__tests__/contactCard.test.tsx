import { cleanup, render, screen } from '@testing-library/react';
import { afterEach, describe, expect, it } from 'vitest';

import ContactCard from '../components/ContactCard';

const contactInfo = {
  email: 'jan.kowalski+imp@put.edu.pl',
  phone: '+48 600 123 456',
  websiteUrl: 'https://example.com/jan-kowalski',
};

const emailPattern =
  /^[A-Za-z0-9.!#$%&'*+/=?^_`{|}~-]+@[A-Za-z0-9](?:[A-Za-z0-9-]{0,61}[A-Za-z0-9])?(?:\.[A-Za-z0-9](?:[A-Za-z0-9-]{0,61}[A-Za-z0-9])?)+$/;
const phonePattern = /^\+[\d\s()-]+$/;
const websitePattern = /^https?:\/\/\S+$/;

afterEach(() => {
  cleanup();
});

describe('ContactCard', () => {
  it('renders the contact section heading', () => {
    render(<ContactCard {...contactInfo} />);

    expect(screen.getByText('Kontakt')).toBeInTheDocument();
  });

  it('renders email, phone and website links with the correct href values', () => {
    render(<ContactCard {...contactInfo} />);

    expect(screen.getByRole('link', { name: emailPattern })).toHaveAttribute(
      'href',
      `mailto:${contactInfo.email}`
    );
    expect(screen.getByRole('link', { name: phonePattern })).toHaveAttribute(
      'href',
      `tel:${contactInfo.phone}`
    );
    expect(screen.getByRole('link', { name: websitePattern })).toHaveAttribute(
      'href',
      contactInfo.websiteUrl
    );
  });
});
