import { render, screen } from '@testing-library/react';
import { describe, expect, it } from 'vitest';
import AboutCard from '../components/AboutCard';
import BaseInfoCard from '../components/BaseInfoCard';
import ConsultationCard from '../components/ConsultationsCard';
import ContactCard from '../components/ContactCard';
import { LecturerSchema } from '../schemas/LecturerSchema';

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

describe('lecturer cards', () => {
  it('renders base information and the lecturer photo', () => {
    render(<BaseInfoCard {...baseInfo} />);

    expect(screen.getByText(baseInfo.title)).toBeTruthy();
    expect(screen.getByText(baseInfo.name)).toBeTruthy();
    expect(screen.getByRole('img', { name: 'lecturer face' }).getAttribute('src')).toBe(
      baseInfo.photoUrl
    );
  });

  it('renders contact details with the correct link targets', () => {
    render(<ContactCard {...contactInfo} />);

    expect(screen.getByText('Kontakt')).toBeTruthy();
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

  it('renders the lecturer description', () => {
    const description = 'Prowadzi zajęcia z programowania i baz danych.';

    render(<AboutCard description={description} />);

    expect(screen.getByText('O prowadzącym')).toBeTruthy();
    expect(screen.getByText(description)).toBeTruthy();
  });

  it('renders the consultations section', () => {
    render(<ConsultationCard />);

    expect(screen.getByText('Konsultacje')).toBeTruthy();
    expect(screen.getByText('jestem godzina konsultacji')).toBeTruthy();
  });
});

describe('LecturerSchema', () => {
  it('accepts a valid lecturer payload', () => {
    const lecturer = {
      id: 67,
      slug: 'jan-kowalski',
      baseInfo,
      contactInfo,
      description: 'Doktor inżynier specjalizujący się w programowaniu.',
    };

    expect(LecturerSchema.parse(lecturer)).toEqual(lecturer);
  });

  it('rejects invalid lecturer contact and base information', () => {
    const invalidLecturer = {
      id: 67,
      slug: 'jan-kowalski',
      baseInfo: { ...baseInfo, photoUrl: 'not-a-url' },
      contactInfo: { ...contactInfo, email: 'not-an-email' },
      description: 'Opis',
    };

    expect(() => LecturerSchema.parse(invalidLecturer)).toThrow();
  });
});
