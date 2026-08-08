import AboutLecturer from '@/components/lecturer/aboutLecturer';
import Consultations from '@/components/lecturer/Consultations';
import LecturerCard from '@/components/lecturer/lecturerCard';
import LecturerContact from '@/components/lecturer/lecturerContact';
import ListLecturerCourses from '@/components/lecturer/lecturerCourseList';
import TimeTable from '@/components/lecturer/timeTable';
import { useLecturer } from '@/hooks/lecturer/useLecturer';
import { useParams } from '@tanstack/react-router';

export default function LecturerPage() {
  const { slug } = useParams({ from: '/lecturer/$slug' });
  const { data, isLoading, isError } = useLecturer(slug);

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (isError) {
    return <div>Error</div>;
  }

  if (!data) {
    return <div>No lecturer found</div>;
  }

  return (
    <div className="flex flex-col gap-10 mx-40 my-20">
      <div className="flex flex-row items-start gap-4 justify-around">
        <LecturerCard
          title={`${data.base_info.title}`}
          name={`${data.base_info.name}`}
          photo_url={`${data.base_info.photo_url}`}
        />
        <LecturerContact email={`${data.contact.email}`} phone={`${data.contact.phone}`} />
      </div>

      <AboutLecturer description={data.description} />

      <div className="flex flex-row items-start gap-4 justify-around">
        <ListLecturerCourses />
        <Consultations />
      </div>
      <TimeTable />
    </div>
  );
}
