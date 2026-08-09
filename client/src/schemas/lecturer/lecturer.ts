import { z } from 'zod';
import { LecturerCardSchema } from './lecturerCard';
import { ContactInfoSchema } from './contactInfo';
import { LecturerCourseSchema } from './lecturerCourse';
export const LecturerSchema = z.object({
  id: z.int(),
  slug: z.string(),
  base_info: LecturerCardSchema,
  contact: ContactInfoSchema,
  description: z.string(),
  // CZY TO MA BYĆ
  opinions: z.array(z.object({ ocena: z.int().min(1).max(5), tresc: z.string() })),
  // DO ZMIANY
  consultations: z.object({
    time: z.string(),
    interval: z.string(),
    weekday: z.string(),
    place: z.string(),
    last_updated: z.string(),
  }),
  lecturers_courses: z.array(LecturerCourseSchema),
  // I think it would be better do use USOS ical
  timetable: z.array(
    z.object({
      day: z.string(),
      time: z.string(),
      przedmiot: z.string(),
      sala: z.string(),
      typ: z.string(),
    })
  ),
});

export type Lecturer = z.infer<typeof LecturerSchema>;
