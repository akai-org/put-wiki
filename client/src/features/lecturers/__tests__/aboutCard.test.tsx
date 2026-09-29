import { render, screen } from '@testing-library/react';
import { describe, expect, it } from 'vitest';

import AboutCard from '../components/AboutCard';

const description = 'Prowadzi zajęcia z programowania i baz danych.';

describe('AboutCard', () => {
  it('should render the provided description text', () => {
    render(<AboutCard description={description} />);

    expect(screen.getByText(description)).toBeInTheDocument();
  });
});
