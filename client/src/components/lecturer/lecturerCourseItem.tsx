import type { LecturerCourse } from '@/schemas/lecturer/lecturerCourse';

export default function LecturerCourseItem({ course }: { course: LecturerCourse }) {
  return (
    <div>
      {course.nazwa} ({course.typ}){' '}
    </div>
  );
}
