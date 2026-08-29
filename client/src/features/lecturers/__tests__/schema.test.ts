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

  it('rejects invalid lecturer contact and base information', () => {
    const invalidLecturer = {
      id: 67,
      slug: 'jan-kowalski',
      baseInfo: { ...baseInfo, photoUrl: 'not-a-url' },
      contactInfo: { ...contactInfo, email: 'not-an-email' },
      description: 'Opis',
    };

    expect(() => lecturerSchema.parse(invalidLecturer)).toThrow();
  });
});
