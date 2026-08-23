import TableOfCourses from './components/TableOfCourses';
export type { Semester } from './components/TableOfCourses';
import { useDegreeCourseQuery } from './api/useDegreeCourseQuery';
import { degreeCourseQueries } from '@/features/degree-courses/api/getDegreeCourse';

export { TableOfCourses, useDegreeCourseQuery, degreeCourseQueries };
