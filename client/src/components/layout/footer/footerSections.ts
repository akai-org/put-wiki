export interface FooterLink {
  label: string;
  href: string;
}
export interface FooterSection {
  id: number;
  title: string;
  links: FooterLink[];
}
export const footerSections: FooterSection[] = [
  {
    id: 1,
    title: 'Kontakt',
    links: [
      { label: 'Email', href: '/contact/email' },
      { label: 'Telefon', href: '/contact/phone' },
    ],
  },
  {
    id: 2,
    title: ' O nas',
    links: [
      { label: 'AKAI', href: '/about/akai' },
      { label: 'Politechnika Poznańska', href: '/about/pp' },
    ],
  },
  {
    id: 3,
    title: 'Przydatne rzeczy',
    links: [
      { label: 'Kierunki studiów', href: '/useful/courses' },
      { label: 'Prowadzący zajęcia', href: '/useful/lecturers' },
      { label: 'Przedmioty', href: '/useful/subjects' },
      { label: 'Szukaj', href: '/useful/search' },
    ],
  },
];
