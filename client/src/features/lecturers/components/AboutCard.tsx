import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card';

interface AboutCardDescriptionProps {
  description: string;
}

export default function AboutCard({ description }: AboutCardDescriptionProps) {
  return (
    <Card className="self-stretch">
      <CardHeader>
        <CardTitle>O prowadzącym</CardTitle>
      </CardHeader>
      <CardContent>{description}</CardContent>
    </Card>
  );
}
