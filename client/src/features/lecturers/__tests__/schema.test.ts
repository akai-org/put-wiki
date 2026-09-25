import { describe, expect, it } from 'vitest';

import { lecturerSchema } from '../schemas/lecturerSchema';

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

describe('lecturerSchema', () => {
  it('accepts a valid lecturer payload', () => {
    const lecturer = {
      id: 67,
      slug: 'jan-kowalski',
      baseInfo,
      contactInfo,
      description: 'Doktor inżynier specjalizujący się w programowaniu.',
    };

    expect(lecturerSchema.parse(lecturer)).toEqual(lecturer);
  });

  it('rejects an invalid base information field', () => {
    const invalidLecturer = {
      id: 67,
      slug: 'jan-kowalski',
      baseInfo: { ...baseInfo, photoUrl: 'not-a-url' },
      contactInfo,
      description: 'Opis',
    };

    const result = lecturerSchema.safeParse(invalidLecturer);

    expect(result.success).toBe(false);
    if (!result.success) {
      expect(result.error.issues).toContainEqual(
        expect.objectContaining({
          path: ['baseInfo', 'photoUrl'],
          message: 'Invalid URL',
        })
      );
    }
  });

  it('rejects an invalid contact information field', () => {
    const invalidLecturer = {
      id: 67,
      slug: 'jan-kowalski',
      baseInfo,
      contactInfo: { ...contactInfo, email: 'not-an-email' },
      description: 'Opis',
    };

    const result = lecturerSchema.safeParse(invalidLecturer);

    expect(result.success).toBe(false);
    if (!result.success) {
      expect(result.error.issues).toContainEqual(
        expect.objectContaining({
          path: ['contactInfo', 'email'],
          message: 'Invalid email address',
        })
      );
    }
  });
});
