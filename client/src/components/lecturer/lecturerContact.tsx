import type { ContactInfo } from '@/schemas/lecturer/contactInfo';

export default function LecturerContact({ email, phone, website }: ContactInfo) {
  return (
    <div className="flex-col bg-gray-50 rounded-xl p-12 pt-4 pl-4">
      <p className="mb-3 font-bold text-xl">Kontakt</p>
      <p>
        E-mail: <a href={`mailto:${email}`}>{email}</a>
      </p>
      <p>
        Tel: <a href={`tel:${phone}`}>{phone}</a>
      </p>
      <p>
        Strona internetowa: <a href={`${website}`}>{website}</a>
      </p>
    </div>
  );
}
