import AboutLecturer from '@/components/lecturer/aboutLecturer';
import Consultations from '@/components/lecturer/Consultations';
import LecturerCard from '@/components/lecturer/lecturerCard';
import LecturerContact from '@/components/lecturer/lecturerContact';
import ListLecturerCourses from '@/components/lecturer/lecturerCourseList';
import TimeTable from '@/components/lecturer/timeTable';
import { useLecturer } from '@/hooks/lecturer/useLecturer';
import { useParams } from '@tanstack/react-router';

export default function LecturerPage() {
  const { slug } = useParams({ from: '/lecturers/$slug' });
  const { data, isLoading, isError } = useLecturer(slug);

  if (isLoading) {
    return <div className="text-2xl text-center">Loading...</div>;
  }

  if (isError) {
    return <div className="text-2xl text-center">Error</div>;
  }

  if (!data) {
    return <div className="text-2xl text-center">No lecturer found</div>;
  }

  return (
    <div className="mx-auto my-6 flex w-full max-w-7xl flex-col gap-5 px-4 sm:my-8 sm:px-6 lg:my-10 lg:px-8">
      <div className="grid grid-cols-1 gap-5 md:grid-cols-[7fr_3fr]">
        <LecturerCard
          title={`${data.base_info.title}`}
          name={`${data.base_info.name}`}
          photo_url={`${data.base_info.photo_url}`}
        />
        <LecturerContact
          email={`${data.contact.email}`}
          phone={`${data.contact.phone}`}
          website={`${data.contact.website}`}
        />
      </div>

      <AboutLecturer description={data.description} />

      <div className="grid grid-cols-1 gap-5 lg:grid-cols-2">
        <ListLecturerCourses courseList={data.lecturers_courses} />
        <Consultations
          time={`${data.consultations.time}`}
          interval={`${data.consultations.interval}`}
          weekday={`${data.consultations.weekday}`}
          place={`${data.consultations.place}`}
          last_updated={`${data.consultations.last_updated}`}
        />
      </div>
      <TimeTable />
    </div>
  );
}
