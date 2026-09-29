import { cleanup, render, screen } from '@testing-library/react';
import { afterEach, describe, expect, it } from 'vitest';

import ContactCard from '../components/ContactCard';

afterEach(cleanup);

const contactInfo = {
  email: 'jan.kowalski+imp@put.edu.pl',
  phone: '+48 600 123 456',
  websiteUrl: 'https://example.com/jan-kowalski',
};

describe('ContactCard', () => {
  it('should render the email link', () => {
    render(<ContactCard {...contactInfo} />);

    expect(screen.getByRole('link', { name: contactInfo.email })).toHaveAttribute(
      'href',
      `mailto:${contactInfo.email}`
    );
  });

  it('should render the phone link', () => {
    render(<ContactCard {...contactInfo} />);

    expect(screen.getByRole('link', { name: contactInfo.phone })).toHaveAttribute(
      'href',
      `tel:${contactInfo.phone}`
    );
  });

  it('should render the website link', () => {
    render(<ContactCard {...contactInfo} />);

    expect(screen.getByRole('link', { name: contactInfo.websiteUrl })).toHaveAttribute(
      'href',
      contactInfo.websiteUrl
    );
  });
});
