import { createFileRoute, useParams } from '@tanstack/react-router';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card';
import { Button } from '@/components/ui/Button';
import { userPublicProfileQueries } from '@/features/user-public-profile/api/userPublicProfileQueries';
import { useUserPublicProfileQuery } from '@/features/user-public-profile/api/useUserPublicProfileKeys';
import { Skeleton } from '@/components/ui/skeleton';

export const Route = createFileRoute('/user/$slug')({
  component: UserPublicProfilePage,
  pendingComponent: UserPublicProfilePageSkeleton,
  loader: ({ context: { queryClient }, params: { slug } }) => {
    return queryClient.ensureQueryData(userPublicProfileQueries.bySlug(slug));
    /* return queryClient.query({  // i know this is deprecated but untill we don't have tanstack-query v6 it should stay like this. After v6 change function to .queries
      queryKey: 
      ...userPublicProfileQueries.bySlug(slug),
      staleTime: 'static',
    }); */
  },
});

function UserPublicProfilePage() {
  const { slug } = useParams({ from: '/user/$slug' });
  const { data } = useUserPublicProfileQuery(slug);
  const { nickname, karma, opinions, reactions } = data || {};

  return (
    <div className="flex size-full">
      <Card className="flex size-full max-w-none flex-row gap-12">
        <div className="flex min-w-0 flex-1 flex-col">
          <CardHeader className="flex h-30 flex-1 flex-row items-start">
            <CardTitle className="text-3xl">
              <h1>{nickname}</h1>
            </CardTitle>
            <Button className=" ml-auto" type="button" variant="destructive">
              Zgłoś użytkownika
            </Button>
          </CardHeader>

          <CardContent className="flex flex-1 flex-row gap-8">
            <div className="flex flex-col items-center gap-2 text-center">
              <h2>Karma</h2>
              <p className="text-2xl font-bold">{karma}</p>
            </div>
            <div className="flex flex-col items-center gap-2 text-center">
              <h2>Komentarze</h2>
              <p className="text-2xl font-bold">{opinions}</p>
            </div>
            <div className="flex flex-col items-center gap-2 text-center">
              <h2>Reakcje</h2>
              <p className="text-2xl font-bold">{reactions}</p>
            </div>
          </CardContent>
        </div>
      </Card>
    </div>
  );
}

function UserPublicProfilePageSkeleton() {
  return (
    <div className="flex size-full">
      <Card className="flex size-full max-w-none flex-row gap-12">
        <div className="flex min-w-0 flex-1 flex-col">
          <CardHeader className="flex h-30 flex-1 flex-row items-start">
            <Skeleton className="h-6 w-48" />
            <Skeleton className=" ml-auto h-8 w-20" />
          </CardHeader>
          <CardContent className="flex flex-1 flex-row gap-8">
            <div className="flex flex-col items-center gap-2 text-center">
              <Skeleton className="h-4 w-16" />
              <Skeleton className="h-6 w-16" />
            </div>
            <div className="flex flex-col items-center gap-2 text-center">
              <Skeleton className="h-4 w-16" />
              <Skeleton className="h-6 w-16" />
            </div>
            <div className="flex flex-col items-center gap-2 text-center">
              <Skeleton className="h-4 w-16" />
              <Skeleton className="h-6 w-16" />
            </div>
          </CardContent>
        </div>
      </Card>
    </div>
  );
}
