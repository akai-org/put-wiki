import { cleanup, render, screen } from '@testing-library/react';
import { afterEach, describe, expect, it } from 'vitest';

import AboutCard from '../components/AboutCard';

afterEach(() => {
  cleanup();
});

describe('AboutCard', () => {
  it('renders the section title', () => {
    render(<AboutCard description="Prowadzi zajęcia z programowania i baz danych." />);

    expect(screen.getByText('O prowadzącym')).toBeTruthy();
  });

  it('renders the provided description text', () => {
    const description = 'Prowadzi zajęcia z programowania i baz danych.';

    render(<AboutCard description={description} />);

    expect(screen.getByText(description)).toBeTruthy();
  });
});
