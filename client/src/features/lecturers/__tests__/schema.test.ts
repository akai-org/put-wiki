import { describe, expect, it, afterEach } from 'vitest';
import { cleanup } from '@testing-library/react';

import { lecturerSchema } from '../schemas/lecturerSchema';

afterEach(cleanup);

const baseInfo = {
  name: 'Jan Kowalski',
  title: 'dr inż.',
  photoUrl: 'https://example.com/jan-kowalski.jpg',
};

const contactInfo = {
  email: 'jan.kowalski@example.com',
  phone: '+48 600 123 456',
  websiteUrl: 'https://example.com/jan-kowalski',
};

const lecturer = {
  id: 67,
  slug: 'jan-kowalski',
  baseInfo,
  contactInfo,
  description: 'Doktor inżynier specjalizujący się w programowaniu.',
};

describe('lecturerSchema', () => {
  it('should accept a valid lecturer', () => {
    expect(lecturerSchema.safeParse(lecturer).success).toBe(true);
  });

  it('should reject an invalid lecturer with invalid photo URL', () => {
    const invalidLecturer = {
      ...lecturer,
      baseInfo: {
        ...baseInfo,
        photoUrl: 'invalid-url',
      },
    };

    expect(lecturerSchema.safeParse(invalidLecturer).success).toBe(false);
  });

  it('should reject an invalid lecturer with invalid email', () => {
    const invalidLecturer = {
      ...lecturer,
      contactInfo: {
        ...contactInfo,
        email: 'invalid-email',
      },
    };

    expect(lecturerSchema.safeParse(invalidLecturer).success).toBe(false);
  });

  it('should allow lecturer with no title', () => {
    const lecturerWithoutTitle = {
      ...lecturer,
      baseInfo: {
        ...baseInfo,
        title: '',
      },
    };

    expect(lecturerSchema.safeParse(lecturerWithoutTitle).success).toBe(true);
  });
});
