import { z } from 'zod';
import { LecturerCardSchema } from './lecturerCard';
import { ContactInfoSchema } from './contactInfo';
//import { LecturerCourseSchema } from './lecturerCourse';
export const LecturerSchema = z.object({
  id: z.int(),
  slug: z.string(),
  baseInfo: LecturerCardSchema,
  contact: ContactInfoSchema,
  description: z.string(),

  //lecturersCourses: z.array(LecturerCourseSchema),
});

export type Lecturer = z.infer<typeof LecturerSchema>;
