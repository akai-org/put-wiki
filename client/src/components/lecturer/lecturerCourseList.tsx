import type { LecturerCourse } from '@/schemas/lecturer/lecturerCourse';
import LecturerCourseItem from './lecturerCourseItem';

interface LecturerCourseListProps {
  courseList: LecturerCourse[];
}

export default function ListLecturerCourses({ courseList }: LecturerCourseListProps) {
  return (
    <div className="flex flex-col gap-2 p-4 bg-primary text-primary-foreground rounded-xl">
      <p className="text-2xl">Prowadzone przemdioty</p>
      <div>
        <p className="font-bold text-xl">2026/2027</p>
        {courseList
          .filter((c) => c.semestr === '2026/2027')
          .map((c) => (
            <LecturerCourseItem key={c.nazwa + c.semestr} course={c} />
          ))}
      </div>
      <div>
        <p className="font-bold text-xl">Poprzednie semsetry</p>
        {courseList
          .filter((c) => c.semestr !== '2025/2026')
          .map((c) => (
            <LecturerCourseItem key={c.nazwa} course={c} />
          ))}
      </div>
    </div>
  );
}
