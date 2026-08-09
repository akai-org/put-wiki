import type { LecturerCourse } from '@/schemas/lecturer/lecturerCourse';
import LecturerCourseItem from './lecturerCourseItem';

export default function ListLecturerCourses({ courseList }: { courseList: Array<LecturerCourse> }) {
  return (
    <div className="flex flex-col gap-2 p-4 bg-gray-50 rounded-xl">
      <p className="text-2xl">Prowadzone przemdioty</p>
      <div>
        <p className="font-bold text-xl">2025/2026</p>
        {courseList
          .filter((c) => c.semestr === 3)
          .map((c) => (
            <LecturerCourseItem key={c.nazwa + c.semestr} course={c} />
          ))}
      </div>
      <div>
        <p className="font-bold text-xl">Poprzednie semsetry</p>
        {courseList
          .filter((c) => c.semestr !== 3)
          .map((c) => (
            <LecturerCourseItem key={c.nazwa} course={c} />
          ))}
      </div>
    </div>
  );
}
