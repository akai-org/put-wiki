import { createFileRoute, Outlet, redirect } from '@tanstack/react-router';
import { queries } from '@/features/auth';

// Pathless layout route - every route placed in the _authenticated/ directory requires a logged in user
export const Route = createFileRoute('/_authenticated')({
  beforeLoad: async ({ context: { queryClient }, location }) => {
    const user = await queryClient.ensureQueryData(queries.user());

    if (!user) {
      throw redirect({ to: '/login', search: { redirect: location.href } });
    }

    return { user };
  },
  component: Outlet,
});
