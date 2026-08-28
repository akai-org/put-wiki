import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card';

interface AboutCardDescription {
  description: string;
}

export default function AboutCard({ description }: AboutCardDescription) {
  return (
    <Card className="self-stretch">
      <CardHeader>
        <CardTitle>O prowadzącym</CardTitle>
      </CardHeader>
      <CardContent>{description}</CardContent>
    </Card>
  );
}
