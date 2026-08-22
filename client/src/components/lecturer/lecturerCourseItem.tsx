import type { LecturerCourse } from '@/schemas/lecturer/lecturerCourse';

interface LecturerCourseItemProps {
  course: LecturerCourse;
}

export default function LecturerCourseItem({ course }: LecturerCourseItemProps) {
  return (
    <div>
      {course.nazwa} ({course.typ}){' '}
    </div>
  );
}
