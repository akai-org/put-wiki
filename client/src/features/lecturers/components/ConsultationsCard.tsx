import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card';

export default function ConsultationCard() {
  return (
    <Card>
      <CardHeader>
        <CardTitle>Konsultacje</CardTitle>
      </CardHeader>
      <CardContent>
        <p className="text-center">
          Widzisz błąd / dane są nieporawne{' '}
          <a href="mailto:idk@putwiki.com">
            <u>zgłoś to tutaj</u>
          </a>
        </p>
      </CardContent>
    </Card>
  );
}
