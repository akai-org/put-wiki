import type { ContactInfo } from '@/schemas/lecturer/contactInfo';

export default function LecturerContact({ email, phone, website }: ContactInfo) {
  return (
    <div className="flex-col bg-gray-50 rounded-xl p-12 pt-4 pl-4">
      <p className="mb-3 font-bold text-xl">Kontakt</p>
      <p className="break-words">
        E-mail:{' '}
        <a className="break-words" href={`mailto:${email}`}>
          {email}
        </a>
      </p>
      <p>
        Tel: <a href={`tel:${phone}`}>{phone}</a>
      </p>
      <p className="break-words">
        Strona internetowa:{' '}
        <a className="break-words" href={`${website}`}>
          {website}
        </a>
      </p>
    </div>
  );
}
