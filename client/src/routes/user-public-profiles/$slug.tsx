import { createFileRoute } from '@tanstack/react-router';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card';
import { Button } from '@/components/ui/Button';

export const Route = createFileRoute('/user-public-profiles/$slug')({
  component: UserPublicProfilePage,
});

export default function UserPublicProfilePage() {
  return (
    <Card>
      <img alt="User Avatar" src="" />
      <CardHeader>
        <CardTitle>nickname</CardTitle>
        <Button type="button">Zgłoś użytkownika</Button>
      </CardHeader>
      <CardContent>
        <Card>
          <CardTitle>Karma</CardTitle>
          <CardContent>10</CardContent>
        </Card>
        <Card>
          <CardTitle>Komentarze</CardTitle>
          <CardContent>10</CardContent>
        </Card>
        <Card>
          <CardTitle>Reakcje</CardTitle>
          <CardContent>10</CardContent>
        </Card>
      </CardContent>
    </Card>
  );
}
