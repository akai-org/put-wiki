import ContactCard from '@/features/lecturers/components/ContactCard';
import BaseInfoCard from '@/features/lecturers/components/BaseInfoCard';
import { createFileRoute, useParams } from '@tanstack/react-router';
import { AboutCard, ConsultationCard } from '@/features/lecturers';
import { lecturerQueries } from '@/features/lecturers/api/lecturerQueries';
import { useLecturerQuery } from '@/features/lecturers/api/useLecturerQuery';
import LecturersCoursesCard from '@/features/lecturers/components/LecturersCoursesCard';

export const Route = createFileRoute('/lecturers/$slug')({
  component: LecturerPage,
  pendingComponent: LecturerPageSkeleton,
  loader: ({ context: { queryClient }, params: { slug } }) => {
    return queryClient.ensureQueryData(lecturerQueries.bySlug(slug));
  },
});

function LecturerPage() {
  const { slug } = useParams({ from: '/lecturers/$slug' });
  const { data } = useLecturerQuery(slug);

  if (!data) {
    return null;
  }

  return (
    <div className="mt-4 w-full mx-auto max-w-7xl">
      <div className="grid grid-cols-1 self-stretch md:grid-cols-[7fr_3fr]">
        <BaseInfoCard {...data.baseInfo} />
        <ContactCard {...data.contactInfo} />
      </div>
      <AboutCard description={data.description} />
      <div className="grid grid-cols-1 self-stretch md:grid-cols-2">
        <LecturersCoursesCard />
        <ConsultationCard />
      </div>
    </div>
  );
}

function LecturerPageSkeleton() {
  return (
    <div aria-label="Loading lecturer" className="mx-auto mt-4 w-full max-w-7xl animate-pulse">
      <div className="grid grid-cols-1 self-stretch md:grid-cols-[7fr_3fr]">
        <div className="m-3 h-56 rounded-xl bg-muted" />
        <div className="m-3 h-56 rounded-xl bg-muted" />
      </div>
      <div className="m-3 h-40 rounded-xl bg-muted" />
      <div className="grid grid-cols-1 self-stretch md:grid-cols-2">
        <div className="m-3 h-56 rounded-xl bg-muted" />
        <div className="m-3 h-56 rounded-xl bg-muted" />
      </div>
    </div>
  );
}
