import AboutLecturer from '@/components/lecturer/aboutLecturer';
import Consultations from '@/components/lecturer/Consultations';
import LecturerCard from '@/components/lecturer/lecturerCard';
import LecturerContact from '@/components/lecturer/lecturerContact';
import ListLecturerCourses from '@/components/lecturer/lecturerCourseList';
import TimeTable from '@/components/lecturer/timeTable';
// import { useParams } from '@tanstack/react-router';

export default function LecturerPage() {
  //  const { slug } = useParams({ from: '/lecturer/$slug' });

  return (
    <div className="flex flex-col gap-10 mx-40 my-20">
      <div className="flex flex-row items-start gap-4 justify-around">
        <LecturerCard title="dr hab. inż." name="Mateusz" surname="Kowalski" />
        <LecturerContact email={'test@test.com'} phone={'123 456 678'} />
      </div>

      <AboutLecturer description={'description'} />

      <div className="flex flex-row items-start gap-4 justify-around">
        <ListLecturerCourses />
        <Consultations />
      </div>
      <TimeTable />
    </div>
  );
}
