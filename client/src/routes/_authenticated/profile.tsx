import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/_authenticated/profile')({
  component: ProfilePage,
});

function ProfilePage() {
  const { user } = Route.useRouteContext();

  return (
    <div className="flex w-full max-w-300 flex-col gap-4 px-8 py-12 font-serif md:px-16">
      <h1 className="text-3xl font-bold">Mój profil</h1>
      <p className="break-all text-muted-foreground">ID użytkownika: {user.userId}</p>
    </div>
  );
}
