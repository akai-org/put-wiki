import { createFileRoute } from '@tanstack/react-router';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card';
import { Button } from '@/components/ui/Button';

export const Route = createFileRoute('/user-public-profiles/$slug')({
  component: UserPublicProfilePage,
});

function UserPublicProfilePage() {
  return (
    <div className="flex size-full">
      <Card className="flex size-full max-w-none flex-row gap-12">
        <img alt="User Avatar" className="size-48 shrink-0 rounded-full" src="" />
        <div className="flex min-w-0 flex-1 flex-col">
          <CardHeader className="flex h-30 flex-1 flex-row items-start">
            <CardTitle className="text-3xl">
              <h1>nickname</h1>
            </CardTitle>
            <Button className="ml-auto bg-red-500 hover:bg-red-600" type="button">
              Zgłoś użytkownika
            </Button>
          </CardHeader>

          <CardContent className="flex flex-1 flex-row gap-8">
            <div className="flex flex-col items-center gap-2 text-center">
              <h2>Karma</h2>
              <p className="text-2xl font-bold">10</p>
            </div>
            <div className="flex flex-col items-center gap-2 text-center">
              <h2>Komentarze</h2>
              <p className="text-2xl font-bold">10</p>
            </div>
            <div className="flex flex-col items-center gap-2 text-center">
              <h2>Reakcje</h2>
              <p className="text-2xl font-bold">10</p>
            </div>
          </CardContent>
        </div>
      </Card>
    </div>
  );
}
