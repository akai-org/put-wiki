import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card';
import type { ContactInfo } from '../schemas/contactInfoSchema';

export default function ContactCard({ ...contactInfo }: ContactInfo) {
  return (
    <Card>
      <CardHeader>
        <CardTitle>Kontakt</CardTitle>
      </CardHeader>
      <CardContent>
        <p className="break-words">
          E-mail:{' '}
          <a className="break-words" href={`mailto:${contactInfo.email}`}>
            {contactInfo.email}
          </a>
        </p>
        <p>
          Tel: <a href={`tel:${contactInfo.phone}`}>{contactInfo.phone}</a>
        </p>
        <p className="break-words">
          Strona internetowa:{' '}
          <a className="break-words" href={`${contactInfo.websiteUrl}`}>
            {contactInfo.websiteUrl}
          </a>
        </p>
      </CardContent>
    </Card>
  );
}
