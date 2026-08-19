import type { ContactInfo } from '@/schemas/lecturer/contactInfo';

interface LecturerContactProps extends ContactInfo {}

export default function LecturerContact({ email, phone, website }: LecturerContactProps) {
  return (
    <div className="flex-col bg-primary text-primary-foreground rounded-xl p-12 pt-4 pl-4">
      <p className="mb-3 font-bold text-xl">Kontakt</p>
      <p className="wrap-break-words">
        E-mail:{' '}
        <a className="wrap-break-words" href={`mailto:${email}`}>
          {email}
        </a>
      </p>
      <p>
        Tel: <a href={`tel:${phone}`}>{phone}</a>
      </p>
      <p className="wrap-break-words">
        Strona internetowa:{' '}
        <a className="wrap-break-words" href={`${website}`}>
          {website}
        </a>
      </p>
    </div>
  );
}
