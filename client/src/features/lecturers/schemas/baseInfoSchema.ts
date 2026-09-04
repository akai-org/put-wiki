import { z } from 'zod';

export const baseInfoSchema = z.object({
  name: z.string(),
  title: z.string(),
  photoUrl: z.url(),
});

export type BaseInfo = z.infer<typeof baseInfoSchema>;
