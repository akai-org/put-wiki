import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card';
export default function LecturersCoursesCard() {
  return (
    <Card>
      <CardHeader>
        <CardTitle>Prowadzone przedmioty</CardTitle>
      </CardHeader>
      <CardContent>
        <ul className="font-medium">Obecny semestr 2026/2027</ul>
        <li>kurs</li>
        <li>kurs</li>
        <ul className="font-medium">Poprzednie semestry</ul>
        <li>kurs</li>
        <li>kurs</li>
        <li>kurs</li>
      </CardContent>
    </Card>
  );
}
