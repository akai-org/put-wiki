import { createFileRoute, useParams } from '@tanstack/react-router';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card';
import { Skeleton } from '@/components/ui/Skeleton';
import { useSuspenseQuery } from '@tanstack/react-query';
import {
  userPublicProfileQueries,
  UserPublicProfileStats,
  UserPublicProfileStatus,
} from '@/features/user-public-profile';
import { Button } from '@/components/ui/Button';
import { Container } from '@/components/ui/Container';
export const Route = createFileRoute('/profiles/$slug')({
  component: UserPublicProfilePage,
  pendingComponent: UserPublicProfilePageSkeleton,
  loader: ({ context: { queryClient }, params: { slug: nicknameSlug } }) => {
    return queryClient.query({
      ...userPublicProfileQueries.bySlug(nicknameSlug),
    });
  },
});

function UserPublicProfilePage() {
  const { slug: nicknameSlug } = useParams({ from: '/profiles/$slug' });
  const { data } = useSuspenseQuery(userPublicProfileQueries.bySlug(nicknameSlug));
  const { nickname, joinedDate, userStatus, karma, opinions, reactions } = data || {};

  return (
    <Container>
      <Card className="relative w-full items-center justify-center">
        <Button className="absolute top-4 right-4" size="sm" type="button" variant="destructive">
          Report
        </Button>
        <div className="flex w-full max-w-5xl flex-col gap-2">
          <CardHeader className="flex h-auto items-center justify-center gap-6">
            <CardTitle className="text-5xl">
              <p>{nickname}</p>
            </CardTitle>
          </CardHeader>
          <CardContent>
            <UserPublicProfileStatus joinedDate={joinedDate} userStatus={userStatus} />
          </CardContent>
          <CardContent>
            <UserPublicProfileStats karma={karma} opinions={opinions} reactions={reactions} />
          </CardContent>
        </div>
      </Card>
    </Container>
  );
}

function UserPublicProfilePageSkeleton() {
  return (
    <Container>
      <Card className="relative w-full items-center justify-center">
        <Skeleton className="absolute top-4 right-4 h-9 w-16" />
        <div className="flex w-full max-w-5xl flex-col gap-2">
          <CardHeader className="flex h-auto items-center justify-center gap-6">
            <Skeleton className="h-12 w-64" />
          </CardHeader>
          <CardContent className="flex justify-center gap-8">
            <div className="flex items-center gap-2">
              <Skeleton className="size-4" />
              <Skeleton className="h-4 w-20" />
              <Skeleton className="h-4 w-24" />
            </div>
            <div className="flex items-center gap-2">
              <Skeleton className="size-4" />
              <Skeleton className="h-4 w-24" />
            </div>
          </CardContent>
          <CardContent className="mt-4 grid w-full grid-cols-1 gap-y-8 md:grid-cols-3 md:gap-y-0">
            <div className="order-2 flex flex-col items-center gap-4 text-center md:order-0">
              <Skeleton className="h-9 w-32" />
              <Skeleton className="h-9 w-16" />
            </div>
            <div className="order-first flex flex-col items-center gap-3 text-center md:order-0">
              <Skeleton className="h-10 w-24" />
              <Skeleton className="h-10 w-16" />
            </div>
            <div className="order-3 flex flex-col items-center gap-2 text-center md:order-0">
              <Skeleton className="h-9 w-24" />
              <Skeleton className="h-9 w-16" />
            </div>
          </CardContent>
        </div>
      </Card>
    </Container>
  );
}
