import { z } from 'zod';
export const LecturerCourseSchema = z.object({
  nazwa: z.string(),
  semestr: z.int(),
  typ: z.string(),
});

export type LecturerCourse = z.infer<typeof LecturerCourseSchema>;
