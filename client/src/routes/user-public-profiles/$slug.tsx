import { createFileRoute, useParams } from '@tanstack/react-router';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card';
import { Button } from '@/components/ui/Button';
import { userPublicProfileQueries } from '@/features/user-public-profile/api/userPublicProfileQueries';
import { useUserPublicProfileQuery } from '@/features/user-public-profile/api/useUserPublicProfileKeys';

export const Route = createFileRoute('/user-public-profiles/$slug')({
  component: UserPublicProfilePage,
  loader: ({ context: { queryClient }, params: { slug } }) => {
    return queryClient.ensureQueryData(userPublicProfileQueries.bySlug(slug));
  },
});

function UserPublicProfilePage() {
  const { slug } = useParams({ from: '/user-public-profiles/$slug' });
  const { data, isLoading, isError } = useUserPublicProfileQuery(slug);

  if (isLoading) {
    return <div className="flex items-center justify-center text-7xl text-black">Ładowanie...</div>;
  }
  if (isError) {
    //maybe add toast about error here
    return (
      <div className="flex items-center justify-center text-7xl text-black">Wystąpił błąd</div>
    );
  }
  if (!data)
    return (
      <div className="flex items-center justify-center text-7xl   text-black">Brak danych</div>
    );

  return (
    <div className="flex size-full">
      <Card className="flex size-full max-w-none flex-row gap-12">
        <div className="flex min-w-0 flex-1 flex-col">
          <CardHeader className="flex h-30 flex-1 flex-row items-start">
            <CardTitle className="text-3xl">
              <h1>{data.nickname}</h1>
            </CardTitle>
            <Button className="ml-auto bg-red-500 hover:bg-red-600" type="button">
              Zgłoś użytkownika
            </Button>
          </CardHeader>

          <CardContent className="flex flex-1 flex-row gap-8">
            <div className="flex flex-col items-center gap-2 text-center">
              <h2>Karma</h2>
              <p className="text-2xl font-bold">{data.karma}</p>
            </div>
            <div className="flex flex-col items-center gap-2 text-center">
              <h2>Komentarze</h2>
              <p className="text-2xl font-bold">{data.opinions}</p>
            </div>
            <div className="flex flex-col items-center gap-2 text-center">
              <h2>Reakcje</h2>
              <p className="text-2xl font-bold">{data.reactions}</p>
            </div>
          </CardContent>
        </div>
      </Card>
    </div>
  );
}
