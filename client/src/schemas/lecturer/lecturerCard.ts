import { z } from 'zod';

export const LecturerCardSchema = z.object({
  name: z.string(),
  title: z.string(),
  photo_url: z.url(),
});

export type LecturerCard = z.infer<typeof LecturerCardSchema>;
