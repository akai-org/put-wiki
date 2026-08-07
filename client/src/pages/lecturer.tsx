import AboutLecturer from '@/components/lecturer/aboutLecturer';
import ListLecturerCourses from '@/components/lecturer/listLecturerCourses';
import { useParams } from '@tanstack/react-router';

export default function LecturerPage() {
  const { slug } = useParams({ from: '/lecturer/$slug' });

  return (
    <div className="flex flex-col gap-6 mx-40">
      <div className="flex flex-row items-start gap-4 justify-between">
        <div>zdj</div>
        <div>prowadzący {slug}</div>
        <div>Spis treści</div>
      </div>

      <AboutLecturer description={'description'} />

      <div className="flex flex-row gap-4">
        <div className="flex-1 flex-col text-center">
          <div>kontakt</div>
          <ListLecturerCourses course="AIDS" />
          <div>timetable</div>
        </div>
        <div className="flex-1 text-center">comments</div>
      </div>
    </div>
  );
}
