import { cleanup, render, screen } from '@testing-library/react';
import { afterEach, describe, expect, it } from 'vitest';

import BaseInfoCard from '../components/BaseInfoCard';

afterEach(cleanup);

const baseInfo = {
  name: 'Jan Kowalski',
  title: 'dr inż.',
  photoUrl: 'https://example.com/jan-kowalski.jpg',
};

describe('BaseInfoCard', () => {
  it('should render the lecturer title and name', () => {
    render(<BaseInfoCard {...baseInfo} />);

    expect(screen.getByText(baseInfo.title)).toBeInTheDocument();
    expect(screen.getByText(baseInfo.name)).toBeInTheDocument();
  });

  it('should render the lecturer photo', () => {
    render(<BaseInfoCard {...baseInfo} />);

    expect(screen.getByRole('img')).toBeInTheDocument();
    expect(screen.getByRole('img')).toHaveAttribute(
      'src',
      baseInfo.photoUrl
    );
  });

  it('should ensure the photo has an alt attribute',()=>{
    render(<BaseInfoCard {...baseInfo} />);
    expect(screen.getByRole('img')).toSatisfy((img: HTMLImageElement) => img.alt !== '');
  });
});
