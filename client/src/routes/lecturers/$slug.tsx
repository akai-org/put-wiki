import ContactCard from '@/features/lecturers/components/ContactCard';
import BaseInfoCard from '@/features/lecturers/components/BaseInfoCard';
import { createFileRoute, useParams } from '@tanstack/react-router';
import { AboutCard, ConsultationCard } from '@/features/lecturers';
import { useLecturerQuery } from '@/features/lecturers/api/useLecturersQuery';
import LecturersCoursesCard from '@/features/lecturers/components/LecturersCoursesCard';

export const Route = createFileRoute('/lecturers/$slug')({ component: LecturerPage });

function LecturerPage() {
  const { slug } = useParams({ from: '/lecturers/$slug' });
  const { data, isLoading, isError } = useLecturerQuery(slug);

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (isError) {
    return <div>Error</div>;
  }
  if (!data) {
    return <div>Error</div>;
  }

  return (
    <div className="mt-4w-full mx-auto max-w-7xl">
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
