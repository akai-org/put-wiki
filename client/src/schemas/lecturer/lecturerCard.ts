import { z } from 'zod';

export const LecturerCardSchema = z.object({
  title: z.string(),
  name: z.string(),
  surname: z.string(),
});

export type LecturerCard = z.infer<typeof LecturerCardSchema>;
