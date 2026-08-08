import { z } from 'zod';
import { LecturerCardSchema } from './lecturerCard';
import { ContactInfoSchema } from './contactInfo';
export const LecturerSchema = z.object({
  id: z.int(),
  slug: z.string(),
  card: LecturerCardSchema,
  contact: ContactInfoSchema,
  description: z.string(),
  // CZY TO MA BYĆ
  opinions: z.array(z.object({ ocena: z.int().min(1).max(5), tresc: z.string() })),
  // DO ZMIANY
  consultations: z.object({
    godzina: z.string(),
    co_ile: z.string(),
    kiedy: z.string(),
    budynek: z.string(),
  }),
  prowadzone_przedmioty: z.array(
    z.object({ nazwa: z.string(), semestr: z.int(), typ: z.string() })
  ),
  plan_zajec: z.array(
    z.object({
      dzien: z.string(),
      godzina: z.string(),
      przedmiot: z.string(),
      sala: z.string(),
      typ: z.string(),
    })
  ),
});

export type Lecturer = z.infer<typeof LecturerSchema>;
