import { cleanup, render, screen } from '@testing-library/react';
import { afterEach, describe, expect, it } from 'vitest';

import BaseInfoCard from '../components/BaseInfoCard';

const baseInfo = {
  name: 'Jan Kowalski',
  title: 'dr inż.',
  photoUrl: 'https://example.com/jan-kowalski.jpg',
};

afterEach(() => {
  cleanup();
});

describe('BaseInfoCard', () => {
  it('renders the lecturer title and name', () => {
    render(<BaseInfoCard {...baseInfo} />);

    expect(screen.getByText(baseInfo.title)).toBeTruthy();
    expect(screen.getByText(baseInfo.name)).toBeTruthy();
  });

  it('renders the lecturer photo', () => {
    render(<BaseInfoCard {...baseInfo} />);

    expect(screen.getByRole('img', { name: 'lecturer face' }).getAttribute('src')).toBe(
      baseInfo.photoUrl
    );
  });
});
