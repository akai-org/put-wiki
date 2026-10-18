import { useParams } from '@tanstack/react-router';
import { useSuspenseQuery } from '@tanstack/react-query';

import {
  ContactCard,
  BaseInfoCard,
  AboutCard,
  ConsultationCard,
  LecturersCoursesCard,
  lecturerQueries,
} from '@/features/lecturers';
import createRoute from '@/utils/createRoute';

export const Route = createRoute(
  '/lecturers/$slug',
  LecturerPage,
  LecturerPageSkeleton,
  lecturerQueries
);

function LecturerPage() {
  const { slug } = useParams({ from: '/lecturers/$slug' });
  const { data } = useSuspenseQuery(lecturerQueries.bySlug(slug));

  return (
    <div className="mx-auto mt-4 w-full max-w-7xl">
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
